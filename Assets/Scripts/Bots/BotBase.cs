using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using TMPro;
using UnityEngine;

public class BotBase: MonoBehaviour
{
    [Header("Movement Attributes")]
    public float MoveSpeed = 5f;
    public float TurnRateDegPerSecond = 120f;
    public bool UpdatePersonalTargetPos = true;
    public float PersonalTargetUpdateDelay = 3f;

    [Header("Health Attributes")]
    public float StartingHealth = 100f;

    [Header("Meta Attributes")]
    public float PersonalTargetArenaRadius = 8f;
    public float WeaponDurabilityDamageFactor = 0.5f;
    public LayerMask GroundLayer;

    [Header("Knockback Attributes")]
    public float MaxHealthKnockbackMultiplier = 4f;
    public float MinHealthKnockbackMultiplier = .25f;
    public float DamageKnockbackFactor = 5f;

    public float HealthPercentage => CurrentHealth / StartingHealth;
    public float CurrentHealth;

	private bool isDead = false;
    public bool IsDead
	{
		get { return isDead; }
		set
		{ if (!isDead && value)
			{
				isDead = true;
				Death?.Invoke(this);
			}
		}
	}

	public Action DamageTaken = delegate { };
	public Action<BotBase> Death = delegate { };

    public Rigidbody rgbd;

    public TextMeshProUGUI playerName;
    public MeshRenderer beybodyModel;
    public List<MeshRenderer> beybladeModels;

    private List<Weapon> activeWeapons;
    private Transform arenaPlatform;
    private Vector3 targetPosition;
    
    private AnalyticsBoi analyticsBoi = new AnalyticsBoi();

    private void Awake()
    {
        CurrentHealth = StartingHealth;
        arenaPlatform = GameObject.Find("ArenaPlatform").transform;
        rgbd = GetComponent<Rigidbody>();

        activeWeapons = GetComponentsInChildren<Weapon>().Where(w => w.gameObject.activeSelf).ToList();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdatePersonalTarget());
    }

	public void HealDamage(float healAmount)
	{
		healAmount = Mathf.Max(0f, healAmount);
		healAmount *= (HealthPercentage - 0.01f);
		CurrentHealth += healAmount;
		CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, StartingHealth);
	}

    public void TakeDamage(float damageAmount, Vector3 contactPoint, bool doKnockback = true)
    {
        var rawDamageAmount = damageAmount;
        damageAmount = Mathf.Min(damageAmount, StartingHealth);
        damageAmount *= (HealthPercentage - 0.01f);
        CurrentHealth -= damageAmount;

        if (doKnockback)
        {
            var healthKnockbackMultiplier = Mathf.Lerp(MaxHealthKnockbackMultiplier, MinHealthKnockbackMultiplier, HealthPercentage);
            var knockbackVec = (transform.position - contactPoint).normalized;
            knockbackVec.y = Mathf.Max(knockbackVec.y, .15f);
            var damageKnockbackFactor = (rawDamageAmount / StartingHealth) * DamageKnockbackFactor;
            knockbackVec *= damageKnockbackFactor * healthKnockbackMultiplier;
            Debug.Log($"knocked back {name}: {knockbackVec}, healthFactor: {healthKnockbackMultiplier}, damageFactor: {damageKnockbackFactor}");
            rgbd.AddForceAtPosition(knockbackVec, contactPoint, ForceMode.Impulse);
        }

        ApplyWeaponDurabilityDamage(damageAmount);

		DamageTaken?.Invoke();

		analyticsBoi.recordDamageDealt(damageAmount);
    }

    private void ApplyWeaponDurabilityDamage(float damageAmount)
    {
        var weaponDurabilityPercentage = (damageAmount / StartingHealth);
        foreach (var weapon in activeWeapons)
        {
            weapon.CurrentDurability -= weapon.startingDurability * weaponDurabilityPercentage * WeaponDurabilityDamageFactor * UnityEngine.Random.Range(.85f, 1.15f);
            weapon.CurrentDurability = Mathf.Max(weapon.CurrentDurability, 0);
        }
    }

    private void Update()
    {
        var turnLimit = TurnRateDegPerSecond * Time.deltaTime;
        var targetPos = arenaPlatform.position.GetXZ() + (targetPosition / 2f);
        targetPos.y = transform.position.y;

        transform.forward = Vector3.RotateTowards(
            transform.forward.GetXZ().normalized, 
            targetPos - transform.position, 
            turnLimit * Mathf.Deg2Rad, 
            0f);

        Debug.DrawLine(targetPos, targetPos + Vector3.up, Color.green, .1f);
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.green, .1f);
        Debug.DrawLine(transform.position, transform.position + targetPos - transform.position, Color.blue, .1f);

        if (IsOnGround())
        {
            rgbd.velocity = transform.forward * MoveSpeed + Vector3.up * rgbd.velocity.y;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Fallout")
        {
            IsDead = true;
            //TODO Need a better way of getting the player object. I propose linking a PlayerController to each bot. - KT
            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if (player.nickname.Equals(playerName)) AirConsole.instance.Message(player.deviceID, "view:dead_view");
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator UpdatePersonalTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(PersonalTargetUpdateDelay);
            if (UpdatePersonalTargetPos)
            {
                var newPos = UnityEngine.Random.insideUnitCircle * PersonalTargetArenaRadius;
                targetPosition = new Vector3(newPos.x, 0, newPos.y);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Weapon weapon = collision.contacts[0].thisCollider.GetComponentInParent<Weapon>();
		BotBase us = collision.contacts[0].thisCollider.GetComponent<BotBase>();

        GameObject obj2 = collision.contacts[0].thisCollider.gameObject;

        BotBase enemy = collision.collider.GetComponent<BotBase>();
        if(weapon && enemy)
        {
            weapon.OnHitEnemy(collision.contacts[0].point, enemy);
        }
		else if (us && enemy)
		{
			Debug.Log("BOT TO BOT COLLISION");
			enemy.TakeDamage(1f, collision.contacts[0].point);
		}
    }

    private bool IsOnGround()
    {
        var onGround = Physics.Raycast(transform.position, Vector3.up * -1, 1f, GroundLayer);
        Debug.DrawRay(transform.position, Vector3.down * 1f, onGround ? Color.white : Color.red, 0.25f, false);

        return onGround;
    }
}