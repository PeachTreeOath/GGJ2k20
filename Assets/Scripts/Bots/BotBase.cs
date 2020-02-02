using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BotBase: MonoBehaviour
{
    [Header("Movement Attributes")]
    public float MoveSpeed = 5f;
    public float TurnRateDegPerSecond = 120f;
    public bool UpdatePersonalTargetPos = true;
    public float PersonalTargetUpdateDelay = 1f;
    public Transform PersonalRandomTarget;

    [Header("Health Attributes")]
    public float StartingHealth = 100f;

    [Header("Meta Attributes")]
    public float PersonalTargetArenaRadius = 8f;
    public float WeaponDurabilityDamageFactor = 0.5f;

    [Header("Knockback Attributes")]
    public float MaxKnockbackFactor = 4f;
    public float MinKnockbackFactor = .25f;

    public float HealthPercentage => CurrentHealth / StartingHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; set; }

    public List<Transform> Targets = new List<Transform>();

    private Rigidbody rgbd;

    public TextMeshProUGUI playerName;
    public MeshRenderer beybodyModel;
    public List<MeshRenderer> beybladeModels;

    private List<Weapon> activeWeapons;

    private void Awake()
    {
        CurrentHealth = StartingHealth;
        Targets.Add(GameObject.Find("ArenaPlatform").transform);
        Targets.Add(PersonalRandomTarget);
        rgbd = GetComponent<Rigidbody>();

        activeWeapons = GetComponentsInChildren<Weapon>().Where(w => w.gameObject.activeSelf).ToList();
    }

    private void Start()
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

    public void TakeDamage(float damageAmount, Vector3 contactPoint)
    {
        // not a linear damage mechanic, incoming damage is reduced by current damage taken
        // resulting in asymptotic behaviour towards 0
        damageAmount = Mathf.Min(damageAmount, StartingHealth);
        damageAmount *= (HealthPercentage - 0.01f);
        CurrentHealth -= damageAmount;
        var knockbackMultiplier = Mathf.Lerp(MaxKnockbackFactor, MinKnockbackFactor, HealthPercentage);

        rgbd.AddForceAtPosition((transform.position - contactPoint + Vector3.up) * 5, contactPoint, ForceMode.Impulse);
        ApplyWeaponDurabilityDamage(damageAmount);
    }

    private void ApplyWeaponDurabilityDamage(float damageAmount)
    {
        var weaponDurabilityPercentage = (damageAmount / StartingHealth);
        foreach (var weapon in activeWeapons)
        {
            weapon.CurrentDurability -= weapon.startingDurability * weaponDurabilityPercentage * WeaponDurabilityDamageFactor * Random.Range(.85f, 1.15f);
            weapon.CurrentDurability = Mathf.Max(weapon.CurrentDurability, 0);
        }
    }

    private void Update()
    {
        if (Targets.Count > 0)
        {
            var turnLimit = TurnRateDegPerSecond * Time.deltaTime;
            var targetPos = Targets.Aggregate(Vector3.zero, (tAcc, t) => tAcc + t.localPosition).GetXZ() / Targets.Count;

            transform.forward = Vector3.RotateTowards(
                transform.forward.GetXZ(), 
                targetPos - transform.position.GetXZ(), 
                turnLimit * Mathf.Deg2Rad, 
                0f);

            if (IsOnGround())
            {
                rgbd.velocity = transform.forward * MoveSpeed + Vector3.up * rgbd.velocity.y;
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
                PersonalRandomTarget.position = new Vector3(newPos.x, 0, newPos.y);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Weapon weapon = collision.contacts[0].thisCollider.GetComponentInParent<Weapon>();
        BotBase enemy = collision.collider.GetComponent<BotBase>();
        if(weapon && enemy)
        {
            weapon.OnHitEnemy(collision.contacts[0].point, enemy);
        }
    }

    private bool IsOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.up * -1, 1f);
    }
}