using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnDeath;
    public bool destroyOnDeath = false;
    public float baseHealth = 10f;

    private float _curHealth;
    private bool dead = false;

    public void ResetLife()
    {
        ResetLife(baseHealth);
    }

    public void ResetLife(float life)
    {
        _curHealth = life;
        dead = false;
        OnDamage?.Invoke(this);
    }

    protected virtual void Death()
    {
        dead = true;
        OnDeath?.Invoke(this);
        if(destroyOnDeath)
        {
            Destroy(gameObject, 1f);
        }
    }

    public void TakeDamage(float damage)
    {
        _curHealth -= damage;
        OnDamage?.Invoke(this);
        if(_curHealth <= 0 && !dead)
        {
            Death();
        }
    }

    public float GetCurHealth()
    {
        return _curHealth;
    }

    public float GetHealthPercentage()
    {
        return (_curHealth/baseHealth)*100;
    }
}
