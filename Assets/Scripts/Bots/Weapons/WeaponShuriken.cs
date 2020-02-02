using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShuriken : MonoBehaviour
{
    public Rigidbody rigidbody;

    public void Fire(Vector3 direction)
    {
        rigidbody.velocity = direction;
    }
}
