using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Projectile
{
    public float vel;
    public float radius;
    public Collider myCollider;
    public LayerMask botLayer;

    public float lifetime = 4f;

    public float Damage { get; set; }

    protected override void Awake()
    {
        myCollider.enabled = false;
    }

    protected void Start()
    {
        Invoke("TurnOnCollider", 1f);
        StartCoroutine(DestroyAfterLifetime());
    }

    public override void Fire(Vector3 direction)
    {
        body.velocity = direction * vel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BotBase bot = collision.gameObject.GetComponent<BotBase>();

        if (bot != null)
        {
            bot.TakeDamage(Damage, transform.position);

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
