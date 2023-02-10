using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship{
    [RequireComponent(typeof(Rigidbody2D), typeof(HealthBase))]
    public class ShipBase : MonoBehaviour
    {
        public Rigidbody2D rigdbody;
        public HealthBase health;
        public SpriteRenderer spriteRenderer;
        [Header("Movement")]
        public float speed = 5f;
        public float turnSpeed = 5f;
        [Header("Art")]
        public List<ShipSpriteSetup> spriteSetups;

        void OnValidate()
        {
            rigdbody = GetComponent<Rigidbody2D>();
            health = GetComponent<HealthBase>();
        }

        private void Init()
        {
            health.OnDamage += OnDamage;
        }

        private void OrderSpriteSetups()
        {
            spriteSetups.Sort((i, z) => i.healthPercentage - z.healthPercentage);
        }
        
        public void MoveForward()
        {
            rigdbody.AddForce(transform.up * speed);
        }

        public void Turn(int direction)
        {
            transform.Rotate(Vector3.forward * turnSpeed);
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

    [System.Serializable]
    public class ShipSpriteSetup
    {
        [Range(0, 100)]
        public int healthPercentage = 100;
        public Sprite sprite;
    }
}
