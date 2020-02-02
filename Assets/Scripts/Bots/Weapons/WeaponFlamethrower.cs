using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlamethrower : Weapon
{
    private BotBase myBot;

    private void Awake()
    {
        myBot = GetComponentInParent<BotBase>();
    }

    public void OnTriggerStay(Collider other)
    {
        var otherBot = other.GetComponentInParent<BotBase>();
        if(otherBot != null && otherBot != myBot)
        {
            otherBot.TakeDamage(damage * Time.deltaTime, transform.position, false);
        }
    }
}
