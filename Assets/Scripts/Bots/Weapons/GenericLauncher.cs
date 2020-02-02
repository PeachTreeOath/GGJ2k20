using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING THIS IS TUNED AS A CANNON
public class GenericLauncher : Weapon
{
    public GameObject projectilePrefabToLaunch;
    public float secsToFire;
    public GameObject fireTransformGameObject;
    public float speed;

    protected float timeSinceFire;

    void Update()
    {
        timeSinceFire += Time.deltaTime;

        if (timeSinceFire > secsToFire)
        {
            timeSinceFire -= secsToFire;

            var parentTransform = GetComponentInParent<BotBase>().transform;
            Vector3 fireDirection = Quaternion.AngleAxis(15f, Vector3.up) * parentTransform.forward.GetXZ().normalized;
            Vector3 up = Vector3.up;
            GameObject spawnedObj = Instantiate(projectilePrefabToLaunch, fireTransformGameObject.transform.position, Quaternion.LookRotation(fireDirection, up));
            spawnedObj.GetComponent<ExampleBullet>().Fire(spawnedObj.transform.forward * speed);
        }
    }

}
