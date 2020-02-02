using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMine : MonoBehaviour
{
    public float radius;
    public Collider myCollider;
    public float damageAmount = 30f;
    public LayerMask botLayer;

    public float lifetime = 8f;

    void Awake()
    {
        myCollider.enabled = false;
        Invoke("TurnOnCollider", 1f);
    }

    void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        BotBase bot = collision.gameObject.GetComponent<BotBase>();

        if (bot != null)
        {
            bot.TakeDamage(damageAmount, transform.position);

            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ResourceLoader.instance.rocketExplosion, transform.position, Quaternion.identity);
        var bots = Physics.SphereCastAll(transform.position, radius, Vector3.up, botLayer);
        foreach (RaycastHit hit in bots)
        {
            var bot = hit.transform.GetComponent<BotBase>();
            if (bot != null)
            {
                bot.TakeDamage(damageAmount, transform.position);
            }
        }

        Destroy(gameObject);
    }

    public void TurnOnCollider()
    {
        myCollider.enabled = true;
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        Explode();
    }
}
