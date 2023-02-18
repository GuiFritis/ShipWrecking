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
    public int scorePoints = 0;

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

    public void TakeDamage(float damage, IKiller killer = null)
    {
        _curHealth -= damage;
        OnDamage?.Invoke(this);
        if(_curHealth <= 0 && !dead)
        {
            if(killer != null)
            {
                killer.OnKill(this);
            }
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

[System.Serializable]
public class SetupSpriteByHealth
{
    [Range(0, 100)]
    public int healthPercentage = 100;
    public Sprite sprite;
}