using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMine : Weapon
{
    public float aliveTime;
    public ParticleSystem explosion;
    
    // Update is called once per frame
    void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
        { 
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
