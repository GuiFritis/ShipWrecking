using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

namespace Ship
{
    [RequireComponent(typeof(Seeker))]
    public class ShipSeeker : ShipBase
    {
        [Header("Seeking")]
        public Seeker seeker;
        public Transform target;
        public float distanceToNextWaypoint = 3f;
        public float distanceToDestination = 5f;
        public Action<ShipSeeker> onDestinationReached;

        private Path _path;
        private int _currentWaypoint = 0;
        bool _destinationReached = false;
        private Vector2 _direction;
        private Quaternion _turnDirection;

        protected override void OnValidate()
        {
            base.OnValidate();
            seeker = GetComponent<Seeker>();
        }

        void Update()
        {
            if(_path != null && !_destinationReached)
            {
                Seek();
                Move();
            }
        }

        public void CalculatePath()
        {
            seeker.StartPath(transform.position, target.position, OnPathGenerated);
        }

        private void OnPathGenerated(Path path)
        {
            if(!path.error)
            {
                _path = path;
                _currentWaypoint = 0;
            }
        }

        private void Seek()
        {
            if(_currentWaypoint < _path.vectorPath.Count)
            {
                _destinationReached = false;

                LookAtLerped(_path.vectorPath[_currentWaypoint]);

                if(Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]) <= distanceToNextWaypoint)
                {
                    _currentWaypoint++;
                }

                if(Vector2.Distance(transform.position, _path.vectorPath[_path.vectorPath.Count-1]) <= distanceToDestination)
                {
                    DestinationReached();
                }
                else
                {
                    _moving = true;
                }
            }
            else
            {
                _moving = false;
                _destinationReached = true;
            }
        }

        public void LookAtLerped(Vector3 point)
        {
            _direction = point - transform.position;
            _direction.Normalize();
            _turnDirection = Quaternion.Euler(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 90f);

            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                _turnDirection, 
                turnSpeed * Time.deltaTime
            );
        }

        private void DestinationReached()
        {
            onDestinationReached?.Invoke(this);
            _currentWaypoint = _path.vectorPath.Count;
            _moving = false;
            _destinationReached = true;
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, distanceToNextWaypoint);

            if(_path != null)
            {
                if(_currentWaypoint < _path.vectorPath.Count)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(_path.vectorPath[_currentWaypoint], 0.5f);
                }
            }
        }
    }   
}