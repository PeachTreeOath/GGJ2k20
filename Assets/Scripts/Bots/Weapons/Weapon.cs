using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public int Level { get; set; } = 1;
    public WeaponType TypeOfWeapon;

	public float damage;
    public float startingDurability = 100f;

    public float CurrentDurability { get; set; }

    private void Awake()
    {
        CurrentDurability = startingDurability;
    }

    public virtual void OnHitEnemy(Vector3 contactPoint, BotBase botBase)
    {
    }
}
