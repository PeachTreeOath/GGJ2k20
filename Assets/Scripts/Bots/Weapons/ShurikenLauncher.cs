using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenLauncher : GenericLauncher
{
    public int numShurikensToSpawn;

    public float speed;

    // Start is called before the first frame update
    void Update()
    {
        timeSinceFire += Time.deltaTime;

        if (timeSinceFire > secsToFire)
        {
            timeSinceFire -= secsToFire;

            for (int i = 0; i < numShurikensToSpawn; i++)
            {
                Quaternion rotation = Quaternion.Euler(0, i * 15, 0);
                GameObject spawnedObj = Instantiate(projectilePrefabToLaunch, transform.position, rotation);
                spawnedObj.GetComponent<Projectile>().Fire(spawnedObj.transform.forward * speed);
            }
        }
    }
}
