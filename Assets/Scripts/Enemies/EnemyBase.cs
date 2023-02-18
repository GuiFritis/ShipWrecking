using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(HealthBase))]
    public abstract class EnemyBase : MonoBehaviour
    {
        public HealthBase health;
        public Player player;
        public UIFillUpdater uiHealth;

        public abstract void WakeUp();
        public abstract void Sleep();

        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            health.ResetLife();
            health.OnDeath += OnDeath;
            health.OnDamage += OnDamage;
        }

        public virtual void OnDeath(HealthBase hp)
        {
            
        }

        public virtual void OnDamage(HealthBase hp)
        {
            if(uiHealth != null)
            {
                uiHealth.UpdateValue(hp.GetCurHealth()/hp.baseHealth);
            }
        }
    }
}