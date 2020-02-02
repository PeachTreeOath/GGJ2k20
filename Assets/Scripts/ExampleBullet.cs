using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Projectile
{
    public Rigidbody rigidbody;

    public override void Fire(Vector3 direction)
    {
        rigidbody.velocity = direction;
    }
}
