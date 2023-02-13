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

        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            health.ResetLife();
        }

        public virtual void OnDeath()
        {
            if(deathVFX != null)
            {
                Instantiate(deathVFX, transform.position, transform.rotation);
            }
        }
    }
}