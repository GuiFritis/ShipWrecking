using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cannon;

namespace Ship{
    [RequireComponent(typeof(Rigidbody2D), typeof(HealthBase))]
    public class ShipBase : MonoBehaviour
    {
        public HealthBase health;
        public SpriteRenderer spriteRenderer;
        [Space]
        public List<CannonBase> cannons = new List<CannonBase>();
        [Header("Movement")]
        public float turnSpeed = 1f;
        public float friction = 5f;
        public float acceleration = 1f;
        public float maxSpeed = 2.5f;
        [Header("Art")]
        public List<SetupSpriteByHealth> spriteSetups;

        protected float _speed = 0f;
        protected bool _moving = false;

        protected virtual void OnValidate()
        {
            health = GetComponent<HealthBase>();
        }

        void Awake()
        {
            Init();
        }

        #region INIT
        private void Init()
        {
            health.OnDamage += OnDamage;
            OrderSpriteSetups();
        }

        private void OrderSpriteSetups()
        {
            spriteSetups.Sort((i, z) => i.healthPercentage - z.healthPercentage);
        }
        #endregion

        void Update()
        {
            Move();
        }

        public void Move()
        {
            if(_moving)
            {
                _speed = Mathf.Min(_speed + acceleration * Time.deltaTime, maxSpeed);
            }
            else //friction
            {
                _speed = Mathf.Max(_speed - friction * Time.deltaTime, 0);
            }
            if(_speed > 0){
                transform.Translate(Vector2.up * -1 * _speed * Time.deltaTime);
            }
        }

        public void Turn(int direction)
        {
            transform.Rotate(Vector3.forward * turnSpeed * direction);
        }

        public void ShootCannon(CannonSide side)
        {
            if(cannons.Count > 0)
            {
                foreach (var item in cannons)
                {
                    if(item.side == side)
                    {
                        item.Shoot();
                    }
                }
            }
        }

        private void OnDamage(HealthBase hp)
        {
            Sprite sprite = spriteRenderer.sprite;
            foreach (var item in spriteSetups)
            {
                if(hp.GetHealthPercentage() <= item.healthPercentage)
                {
                    sprite = item.sprite;
                    break;
                }
            }
            spriteRenderer.sprite = sprite;
        }

        public void SetMoving(bool moving)
        {
            _moving = moving;
        }
    }
}
