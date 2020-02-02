using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericLauncher : MonoBehaviour
{
    public GameObject projectilePrefabToLaunch;
    public float secsToFire;

    private float timeSinceFire;

    void Update()
    {
        timeSinceFire += Time.deltaTime;

        if (timeSinceFire > secsToFire)
        {
            timeSinceFire -= secsToFire;

            GameObject spawnedObj = Instantiate(projectilePrefabToLaunch, transform.position, projectilePrefabToLaunch.transform.rotation);

            spawnedObj.GetComponent<Projectile>().Fire(transform.forward);
        }
    }
}
