using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public int Level { get; set; } = 1;
    public WeaponType TypeOfWeapon;

	public float damage => damageLevels[Mathf.Min(Level - 1, damageLevels.Length - 1)];
    public float[] damageLevels = new float[0];
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
