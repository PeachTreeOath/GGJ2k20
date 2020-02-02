using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenLauncher : Weapon
{
    public int numShurikensToSpawn;
    public Projectile projectilePrefabToLaunch;
    public float secsToFire;

    protected float timeSinceFire;

    public float speed;

    // Start is called before the first frame update
    void Update()
    {
        if (this.isActiveAndEnabled)
        {
            timeSinceFire += Time.deltaTime;

            if (timeSinceFire > secsToFire)
            {
                timeSinceFire -= secsToFire;

                for (int i = 0; i < numShurikensToSpawn; i++)
                {
                    Quaternion rotation = GetComponentInParent<BotBase>().transform.rotation;
                    rotation *= Quaternion.Euler(0, i * 15, 0);
                    Projectile spawnedObj = Instantiate(projectilePrefabToLaunch, transform.position, rotation);
                    spawnedObj.Damage = damage;
                    spawnedObj.IgnoreTransform = MyBot.transform;
                    spawnedObj.GetComponent<WeaponShuriken>().Fire(spawnedObj.transform.forward * speed);
                }
            }
        }
    }
}
