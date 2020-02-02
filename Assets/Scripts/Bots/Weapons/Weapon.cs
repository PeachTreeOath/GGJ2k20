using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;

    public virtual void OnHitEnemy(Vector3 contactPoint, BotBase botBase)
    {
    }
}
