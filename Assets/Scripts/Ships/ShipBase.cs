using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship{
    [RequireComponent(typeof(Rigidbody2D), typeof(HealthBase))]
    public class ShipBase : MonoBehaviour
    {
        public HealthBase health;
        public SpriteRenderer spriteRenderer;
        [Header("Movement")]
        public float turnSpeed = 5f;
        public float friction = 15f;
        public bool moving = false;
        public float acceleration = 10f;
        public float maxSpeed = 10f;
        [Header("Art")]
        public List<SetupSpriteByHealth> spriteSetups;

        private float _speed = 0f;

        void OnValidate()
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
            if(moving)
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

        private void OnDamage(HealthBase hp)
        {
            Sprite sprite = spriteRenderer.sprite;
            foreach (var item in spriteSetups)
            {
                if(hp.GetHealthPercentage() <= item.healthPercentage)
                {
                    sprite = item.sprite;
                }
            }
            spriteRenderer.sprite = sprite;
        }
    }
}
