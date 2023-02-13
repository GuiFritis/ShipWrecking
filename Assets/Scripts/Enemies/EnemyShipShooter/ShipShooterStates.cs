using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Enemy.ShipShooter
{
    public class ShipShooterStateBase : StateBase
    {
        public EnemyShipShooter shipShooter;
        public ShipShooterStateBase(EnemyShipShooter enemy)
        {
            shipShooter = enemy;
        }
    }

    public class ShipShooterStateSleeping : ShipShooterStateBase
    {
        public ShipShooterStateSleeping(EnemyShipShooter enemy) : base(enemy){}
    }
}