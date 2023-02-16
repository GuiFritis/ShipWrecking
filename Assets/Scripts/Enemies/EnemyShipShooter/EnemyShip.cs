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
        [Min(0.2f)]
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
            ship.SetMoving(false);
        }

        public override void WakeUp()
        {
            _awake = true;
        }
        #endregion

        #region SEEKING
        public void StartSeeking()
        {
            ship.SetMoving(true);
            _seekCoroutine = StartCoroutine(SeekPlayer());
        }

        private IEnumerator SeekPlayer()
        {
            while(_awake)
            {
                ship.CalculatePath();
                yield return new WaitForSeconds(timeToUpdatePath);
            }
        }

        public void StopSeeking()
        {
            if(_seekCoroutine != null)
            {
                StopCoroutine(_seekCoroutine);
            }
            ship.SetMoving(false);
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
            if(Vector2.Angle(transform.up * -1, (player.transform.position - transform.position).normalized) < maxAngleToShoot
                && ship.cannons.Count > 0)
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
            ship.maxSpeed = _maxSpeedBase;
        }
        #endregion

        #region DEAD
        public void Die()
        {
            OnSleep();
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(AnimateDeath());
        }

        private IEnumerator AnimateDeath()
        {
            while(transform.localScale.magnitude > 1.2f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 0.1f);
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }
        #endregion

        public override void OnDeath(HealthBase hp)
        {
            base.OnDeath(hp);
            SwitchState(EnemyShipState.DEAD);
        }

        private void OnDestinationReached(ShipSeeker ship)
        {
            if(_stm.CurrentState.GetType() != typeof(ShipStateShooting))
            {
                SwitchState(EnemyShipState.SHOOTING);
            }
        }

        public void SwitchState(EnemyShipState state)
        {
            _stm.SwitchState(state, this);
        }
    }
}