using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollisionDamage : MonoBehaviour
{
    [Min(0)]
    public float damage = 2f;
    [Min(0)]
    public float selfDamage = 2f;
    public LayerMask hitLayer;
    public HealthBase health;
    public Predicate<GameObject> validateDamage;

    void OnValidate()
    {
        health = GetComponent<HealthBase>();
    }

    void Start()
    {
        validateDamage += ValidateLayer;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(validateDamage.Invoke(other.gameObject))
        {
            other.gameObject.GetComponent<HealthBase>().TakeDamage(damage);
            if(health != null)
            {
                health.TakeDamage(selfDamage);
            }
        }
    }

    private bool ValidateLayer(GameObject other)
    {
        return ((1 << other.layer) & hitLayer.value) > 0;
    }
}
