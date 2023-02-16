using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;
using StateMachine;

namespace Enemy.EnemyShip
{
    public enum EnemyShipState
    {
        SLEEPING,
        SEEKING,
        SHOOTING,
        DEAD
    }

    [RequireComponent(typeof(ShipSeeker))]
    public class EnemyShip : EnemyBase
    {
        public ShipSeeker ship;
        public Player player;
        [Header("Seeking")]
        public float timeToUpdatePath = 0.5f;
        public float distanceToDestination = 4f;
        [Header("Shooting")]
        public float maxDistanceToShoot = 5f;
        public float maxAngleToShoot = 10f;

        private bool _awake = false;
        private Coroutine _seekCoroutine;
        private StateMachine<EnemyShipState> _stm;
        private float _maxSpeedBase;

        protected override void Init()
        {
            base.Init();

            _stm = new StateMachine<EnemyShipState>();
            _stm.Init();
            _stm.RegisterStates(EnemyShipState.SLEEPING, new ShipStateSleeping());
            _stm.RegisterStates(EnemyShipState.SHOOTING, new ShipStateShooting());
            _stm.RegisterStates(EnemyShipState.SEEKING, new ShipStateSeeking());
            _stm.RegisterStates(EnemyShipState.DEAD, new ShipStateDead());

            SwitchState(EnemyShipState.SLEEPING);

            ship.distanceToDestination = distanceToDestination;
            ship.target = player.transform;
            ship.onDestinationReached += OnDestinationReached;

            player.ship.health.OnDeath += hp => SwitchState(EnemyShipState.SLEEPING);
        }

        void Start()
        {
            SwitchState(EnemyShipState.SEEKING);
            _maxSpeedBase = player.ship.maxSpeed;
        }

        void Update() {
            _stm.OnUpdate();
        }

        public override void Sleep()
        {
            _stm.SwitchState(EnemyShipState.SLEEPING);
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
            Debug.Log("Start Seeking");
            _seekCoroutine = StartCoroutine(SeekPlayer());
        }

        private IEnumerator SeekPlayer()
        {
            Debug.Log("Seek Player");
            Debug.Log(_awake);
            while(_awake)
            {
                ship.CalculatePath();
                yield return new WaitForSeconds(timeToUpdatePath);
            }
        }

        public void StopSeeking()
        {
            Debug.Log("Stop Seeking");
            if(_seekCoroutine != null)
            {
                StopCoroutine(_seekCoroutine);
            }
        }
        #endregion

        #region SHOOTING
        public void StartShooting()
        {
            Debug.Log("Start Shooting");
            ship.maxSpeed = player.ship.maxSpeed;
        }

        public void Aim()
        {
            Debug.Log("Aiming");
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
                SwitchState(EnemyShipState.SEEKING);
            }
        }

        public void StopShooting()
        {
            Debug.Log("Stop Shooting");
            ship.maxSpeed = _maxSpeedBase;
        }
        #endregion

        public override void OnDeath(HealthBase hp)
        {
            base.OnDeath(hp);
            SwitchState(EnemyShipState.DEAD);
        }

        private void OnDestinationReached(ShipSeeker ship)
        {
            SwitchState(EnemyShipState.SHOOTING);
        }

        public void SwitchState(EnemyShipState state)
        {
            _stm.SwitchState(state, this);
        }
    }
}