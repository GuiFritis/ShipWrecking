using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Pathfinding;

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
        public AIDestinationSetter destination;
        public StateMachine<ShipShooterState> _stm;

        protected override void Init()
        {
            base.Init();
            _stm = new StateMachine<ShipShooterState>();
            _stm.Init();
        }

        public override void Sleep()
        {
            throw new System.NotImplementedException();
        }

        public override void WakeUp()
        {
            throw new System.NotImplementedException();
        }
    }
}