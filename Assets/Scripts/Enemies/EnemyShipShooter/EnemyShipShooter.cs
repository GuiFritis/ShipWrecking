using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;

namespace Enemy.ShipShooter
{
    public enum ShipShooterState
    {
        SLEEPING,
        PERSUING,
        SHOOTING
    }

    public class EnemyShipShooter : EnemyShip
    {
        public float distanceToShoot = 5f;
        new public ShipSeeker ship;
        public Player player;
        public float timeToUpdatePath = 0.5f;

        private bool _seeking = false;

        protected override void Init()
        {
            base.Init();
            ship.distanceToDestination = distanceToShoot;
            ship.target = player.transform;
        }

        public override void Sleep()
        {
            throw new System.NotImplementedException();
        }

        public override void WakeUp()
        {
            _seeking = true;
            StartCoroutine(SeekPlayer());
        }

        private IEnumerator SeekPlayer()
        {
            while(_seeking)
            {
                ship.CalculatePath();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}