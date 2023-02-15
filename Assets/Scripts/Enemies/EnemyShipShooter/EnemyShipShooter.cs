using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;
using StateMachine;

namespace Enemy.ShipShooter
{
    public enum ShipShooterState
    {
        SLEEPING,
        SEEKING,
        SHOOTING,
        DEAD
    }

    public class EnemyShipShooter : EnemyShip
    {
        public Player player;
        [Header("Shooting")]
        public float distanceToShoot = 4f;
        public float maxDistanceToShoot = 5f;
        public float maxAngleToShoot = 10f;
        [Space]
        public float timeToUpdatePath = 0.5f;

        private bool _awake = false;
        private Coroutine _seekCoroutine;
        private StateMachine<ShipShooterState> _stm;
        private float _maxSpeedBase;

        protected override void Init()
        {
            base.Init();

            _stm = new StateMachine<ShipShooterState>();
            _stm.Init();
            _stm.RegisterStates(ShipShooterState.SLEEPING, new ShipShooterStateSleeping());
            _stm.RegisterStates(ShipShooterState.SHOOTING, new ShipShooterStateShooting());
            _stm.RegisterStates(ShipShooterState.SEEKING, new ShipShooterStateSeeking());
            _stm.RegisterStates(ShipShooterState.DEAD, new ShipShooterStateDead());

            SwitchState(ShipShooterState.SLEEPING);

            ship.distanceToDestination = distanceToShoot;
            ship.target = player.transform;
            ship.onDestinationReached += OnDestinationReached;

            player.ship.health.OnDeath += hp => SwitchState(ShipShooterState.SLEEPING);
        }

        void Start()
        {
            SwitchState(ShipShooterState.SEEKING);
        }

        void Update() {
            _stm.OnUpdate();
        }

        public override void Sleep()
        {
            _stm.SwitchState(ShipShooterState.SLEEPING);
        }

        #region SLEEPING
        public void OnSleep()
        {            
            _awake = false;
        }

        public override void WakeUp()
        {
            _awake = true;
        }
        #endregion

        #region SEEKING
        public void StartSeeking()
        {
            _seekCoroutine = StartCoroutine(SeekPlayer());
        }

        private IEnumerator SeekPlayer()
        {
            while(_awake)
            {
                ship.CalculatePath();
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void StopSeeking()
        {
            if(_seekCoroutine != null)
            {
                StopCoroutine(_seekCoroutine);
            }
        }
        #endregion

        #region SHOOTING
        public void StartShooting()
        {
            ship.maxSpeed = player.ship.maxSpeed;
        }

        public void Aim()
        {
            ship.LookAtLerped(player.transform.position);
            if(Vector2.Angle(transform.up * -1, (player.transform.position - transform.position).normalized) < maxAngleToShoot)
            {
                var direction = player.transform.position - transform.position;
                direction.Normalize();

                ship.cannons[0].transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                ship.ShootCannon(Cannon.CannonSide.FRONT);
            }

            if(Vector2.Distance(transform.position, player.transform.position) > maxDistanceToShoot)
            {
                SwitchState(ShipShooterState.SEEKING);
            }
        }

        public void StopShooting()
        {
            ship.maxSpeed = _maxSpeedBase;
        }
        #endregion

        public override void OnDeath(HealthBase hp)
        {
            base.OnDeath(hp);
            SwitchState(ShipShooterState.DEAD);
        }

        private void OnDestinationReached(ShipSeeker ship)
        {
            SwitchState(ShipShooterState.SHOOTING);
        }

        public void SwitchState(ShipShooterState state)
        {
            _stm.SwitchState(state, this);
        }
    }
}