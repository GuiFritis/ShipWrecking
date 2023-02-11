using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(TrailRenderer))]
public class CannonBallBase : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 4f;
    public float damage = 10f;
    public GameObject hit_vfx;
    public LayerMask hitLayer;

    void OnValidate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        Destroy(this, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * -1 * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(((1<<other.gameObject.layer) & hitLayer) != 0)
        {
            HealthBase hp = other.GetComponent<HealthBase>();
            if(hp != null)
            {
                hp.TakeDamage(damage);
            }
            if(hit_vfx != null)
            {
                Instantiate(hit_vfx, transform.position, transform.rotation);
            }
            Destroy(this);
        }
        
    }
}
