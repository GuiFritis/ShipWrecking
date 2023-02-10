using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship{
    public class ShipBase : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rigdbody;
        [SerializeField]
        private float speed = 5f;
        [SerializeField]
        private float turnSpeed = 5f;

        void OnValidate()
        {
            rigdbody = GetComponent<Rigidbody2D>();
        }
        
        public void MoveForward()
        {
            rigdbody.AddForce(transform.up * speed);
        }

        public void Turn(int direction)
        {
            transform.Rotate(Vector3.forward * turnSpeed);
        }
    }
}
