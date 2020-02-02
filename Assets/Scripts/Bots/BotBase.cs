using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float HealthPercentage => CurrentHealth / StartingHealth;
    public float CurrentHealth { get; private set; }

    public List<Transform> Targets = new List<Transform>();

    private Rigidbody rgbd;

    private void Awake()
    {
        CurrentHealth = StartingHealth;
        Targets.Add(GameObject.Find("ArenaPlatform").transform);
        Targets.Add(PersonalRandomTarget);
        rgbd = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(UpdatePersonalTarget());
    }

    public void TakeDamage(float damageAmount, Vector3 contactPoint)
    {
        // not a linear damage mechanic, incoming damage is reduced by current damage taken
        // resulting in asymptotic behaviour towards 0
        damageAmount = Mathf.Min(damageAmount, StartingHealth);
        damageAmount *= (HealthPercentage - 0.01f);
        CurrentHealth -= damageAmount;

        rgbd.AddForceAtPosition((transform.position - contactPoint + Vector3.up) * 5, contactPoint, ForceMode.Impulse);
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
                var newPos = Random.insideUnitCircle * PersonalTargetArenaRadius;
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