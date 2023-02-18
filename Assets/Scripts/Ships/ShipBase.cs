using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cannon;

namespace Ship{
    [RequireComponent(typeof(Rigidbody2D), typeof(HealthBase), typeof(CollisionDamage))]
    public class ShipBase : MonoBehaviour
    {
        public HealthBase health;
        public CollisionDamage collisionDamage;
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
        public GameObject deathVFX;
        [Space]
        [Tooltip("The margin at wich the collision deals self damage")]
        [Range(0f, 1f)]
        public float marginHitDamage = 0.8f;

        protected float _speed = 0f;
        protected bool _moving = false;

        protected virtual void OnValidate()
        {
            health = GetComponent<HealthBase>();
            collisionDamage = GetComponent<CollisionDamage>();
        }

        void Awake()
        {
            Init();
        }

        #region INIT
        private void Init()
        {
            health.OnDamage += OnDamage;
            health.OnDeath += OnDeath;
            OrderSpriteSetups();
        }

        private void OrderSpriteSetups()
        {
            spriteSetups.Sort((i, z) => i.healthPercentage - z.healthPercentage);
        }
        #endregion

        void Start()
        {
            collisionDamage.validateDamage += DamageIfMaxSpeed;
        }

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

        public void ShootCannon(CannonSide side, IKiller shooter = null)
        {
            if(cannons.Count > 0)
            {
                foreach (var item in cannons)
                {
                    if(item.side == side)
                    {
                        item.Shoot(shooter);
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

        public bool DamageIfMaxSpeed(GameObject other)
        {
            return _speed >= maxSpeed * marginHitDamage;
        }

        public void OnDeath(HealthBase hp)
        {
            if(deathVFX != null)
            {
                Instantiate(deathVFX, transform.position, transform.rotation);
            }
            _moving = false;
            maxSpeed = 0f;
            turnSpeed = 0f;
            cannons.Clear();
        }
    }
}
