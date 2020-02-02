using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShuriken : Projectile
{
    public override void Fire(Vector3 direction)
    {
        body.velocity = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var bot = collision.transform.GetComponent<BotBase>();
        if(bot && bot.transform != IgnoreTransform)
        {
            bot.TakeDamage(Damage, transform.position);

            Destroy(gameObject, 0);
        }
    }
}
