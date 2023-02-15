using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Enemy.ShipShooter
{
    public abstract class ShipShooterStateBase : StateBase
    {
        public EnemyShipShooter shipShooter;

        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            if(objs.Length > 0)
            {
                shipShooter = (EnemyShipShooter)objs[0];
            }
        }
    }

    public class ShipShooterStateSleeping : ShipShooterStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            shipShooter.OnSleep();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            shipShooter.WakeUp();
        }
    }

    public class ShipShooterStateShooting : ShipShooterStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            shipShooter.StartShooting();
        }

        public override void OnStateStay()
        {
            base.OnStateStay();
            shipShooter.Aim();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            shipShooter.StopShooting();
        }
    }

    public class ShipShooterStateSeeking : ShipShooterStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            shipShooter.StartSeeking();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            shipShooter.StopSeeking();
        }
    }

    public class ShipShooterStateDead : ShipShooterStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            shipShooter.OnSleep();
            GameObject.Destroy(shipShooter.gameObject, 2f);
        }
    }
}