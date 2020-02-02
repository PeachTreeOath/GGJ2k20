using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Projectile
{
    public override void Fire(Vector3 direction)
    {
        body.velocity = direction;
    }
}
