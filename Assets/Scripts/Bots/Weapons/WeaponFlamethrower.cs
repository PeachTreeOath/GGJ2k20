using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlamethrower : Weapon
{
	private void Awake()
	{
		TypeOfWeapon = WeaponType.Flamethrower;
	}

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        // if older belongs to player GO consistent damage
    }
}
