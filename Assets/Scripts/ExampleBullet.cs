using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Projectile
{
    public float vel;
    public float radius;
    private Collider myCollider;
    public LayerMask botLayer;

    public float lifetime = 4f;

    private Vector3 moveVelocity;

    protected override void Awake()
    {
        base.Awake();
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
    }

    protected void Start()
    {
        Invoke("TurnOnCollider", .51f);
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Update()
    {
        body.velocity = moveVelocity;
    }

    public override void Fire(Vector3 direction)
    {
        moveVelocity = direction * vel;
        body.velocity = moveVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var bot = collision.transform.GetComponent<BotBase>();

        if (bot && bot.transform != IgnoreTransform)
        {
            Debug.Log("cannon hit");
            bot.TakeDamage(Damage, transform.position);
            Explode();
        }
        else
        {
            Debug.Log("cannon boom");
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ResourceLoader.instance.rocketExplosion, transform.position, Quaternion.identity);
        var bots = Physics.SphereCastAll(transform.position, radius, Vector3.up, botLayer);
        foreach (RaycastHit hit in bots)
        {
            var bot = hit.transform.GetComponent<BotBase>();
            if (bot != null)
            {
                bot.TakeDamage(Damage, transform.position);
            }
        }

        Destroy(gameObject);
    }

    public void TurnOnCollider()
    {
        myCollider.enabled = true;
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        Explode();
    }
}
