using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    public override void Fire(Vector3 direction)
    {
        body.velocity = direction;
    }

    public void Explode()
    {
        Instantiate(ResourceLoader.instance.rocketExplosion);
    }
}
