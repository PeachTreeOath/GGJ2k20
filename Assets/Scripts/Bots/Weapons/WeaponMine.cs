﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMine : MonoBehaviour
{
    public float radius;
    public Collider myCollider;
    public float damageAmount { get; set; }
    public LayerMask botLayer;

    public float lifetime = 8f;

    public float aliveTime;
    public ParticleSystem explosion;
    
    // Update is called once per frame
    void Update()
    {
        aliveTime -= Time.deltaTime;
    }

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
        var bots = Physics.SphereCastAll(transform.position, radius, Vector3.up, botLayer);
        foreach (RaycastHit hit in bots)
        {
            var bot = hit.transform.GetComponent<BotBase>();
            if (bot != null)
            {
                bot.TakeDamage(damageAmount, transform.position);
            }
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
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
