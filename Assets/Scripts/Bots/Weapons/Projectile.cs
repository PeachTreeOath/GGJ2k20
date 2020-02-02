using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    protected Rigidbody body;

    // Start is called before the first frame update
    protected void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    abstract public void Fire(Vector3 direction);

}
