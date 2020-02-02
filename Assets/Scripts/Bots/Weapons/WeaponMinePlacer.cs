using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMinePlacer : Weapon
{
    public int dropTimeFrequency;

    public float currentCountdownTime;

    public GameObject minePrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentCountdownTime = dropTimeFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        currentCountdownTime -= Time.deltaTime;
        if (currentCountdownTime <= 0)
        {
            GameObject newMine = Instantiate(minePrefab, transform.position, transform.rotation);
            currentCountdownTime = dropTimeFrequency;
        }
    }
}
