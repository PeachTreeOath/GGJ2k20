using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShuriken : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float damage = 5f;

    public void Fire(Vector3 direction)
    {
        rigidbody.velocity = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var bot = collision.other.GetComponent<BotBase>();
        if(bot)
        {
            Debug.Log("shuriken hit");
            bot.TakeDamage(damage, transform.position);

            Destroy(gameObject, 0);
        }
    }
}
