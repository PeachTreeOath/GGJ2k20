using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : Weapon
{
    public override void OnHitEnemy(Vector3 contactPoint, BotBase botBase)
    {
        botBase.TakeDamage(damage, contactPoint);
    }
}
