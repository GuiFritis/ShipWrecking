using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(HealthBase))]
    public abstract class EnemyBase : MonoBehaviour
    {
        public HealthBase health;
        public GameObject deathVFX;

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
        }

        public virtual void OnDeath(HealthBase hp)
        {
            if(deathVFX != null)
            {
                Instantiate(deathVFX, transform.position, transform.rotation);
            }
        }
    }
}