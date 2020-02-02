using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING THIS IS TUNED AS A CANNON
public class GenericLauncher : MonoBehaviour
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

            Quaternion rotation = GetComponentInParent<BotBase>().transform.rotation;
            rotation *= Quaternion.Euler(15, 0, 0);
            GameObject spawnedObj = Instantiate(projectilePrefabToLaunch, fireTransformGameObject.transform.position, rotation);
            spawnedObj.GetComponent<ExampleBullet>().Fire(spawnedObj.transform.forward * speed);
        }
    }

}
