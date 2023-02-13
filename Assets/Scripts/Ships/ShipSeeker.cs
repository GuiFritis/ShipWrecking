using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Ship
{
    [RequireComponent(typeof(Seeker))]
    public class ShipSeeker : ShipBase
    {
        [Header("Seeking")]
        public Seeker seeker;
        public Player target;
        public float distanceToNextWaypoint = 3f;
        public float distanceToShoot = 5f;

        private Path _path;
        private int _currentWaypoint = 0;
        bool _destinationReached = false;
        private Vector2 _direction;
        private Quaternion _turnDirection;

        void OnValidate()
        {
            seeker = GetComponent<Seeker>();
        }

        void Start()
        {
            CalculatePath();
        }

        void Update()
        {
            if(_path != null && !_destinationReached)
            {
                Seek();
                Move();
            }
        }

        private void CalculatePath()
        {
            seeker.StartPath(transform.position, target.transform.position, OnPathGenerated);
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
                moving = true;

                _direction = _path.vectorPath[_currentWaypoint] - transform.position;
                _direction.Normalize();
                _turnDirection = Quaternion.Euler(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 90f);

                transform.rotation = Quaternion.Lerp(
                    transform.rotation, 
                    _turnDirection, 
                    turnSpeed * Time.deltaTime
                );

                if(Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]) <= distanceToNextWaypoint)
                {
                    _currentWaypoint++;
                }
            }
            else
            {
                moving = false;
                _destinationReached = true;
            }
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