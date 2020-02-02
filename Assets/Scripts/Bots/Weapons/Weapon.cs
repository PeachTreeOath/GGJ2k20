using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public int Level { get; set; } = 1;
	public WeaponType TypeOfWeapon { get; protected set; }

	public float damage;

    public virtual void OnHitEnemy(Vector3 contactPoint, BotBase botBase)
    {
    }
}
