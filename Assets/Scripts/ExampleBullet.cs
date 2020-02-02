using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Projectile
{
    public float force;
    public float radius;
    public Collider myCollider;
    public Rigidbody rigidbody;

    void Awake()
    {
        Invoke("TurnOnCollider", 1f);
        body = GetComponent<Rigidbody>();
    }

    public override void Fire(Vector3 direction)
    {
        rigidbody.velocity = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BotBase weapon = collision.gameObject.GetComponent<BotBase>();

        if (weapon != null)
        {
            BotBase[] botObjs = FindObjectsOfType<BotBase>();

            Instantiate(ResourceLoader.instance.rocketExplosion, transform.position, Quaternion.identity);
            foreach (BotBase bot in botObjs)
            {
                bot.rgbd.AddExplosionForce(force, transform.position, radius);
            }

            Destroy(gameObject);
        }
    }

    public void TurnOnCollider()
    {
        myCollider.enabled = true;
    }
}
