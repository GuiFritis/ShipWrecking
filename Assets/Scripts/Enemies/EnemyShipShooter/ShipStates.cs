using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Enemy.EnemyShip
{
    public abstract class ShipStateBase : StateBase
    {
        public EnemyShip enemyShip;

        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            if(objs.Length > 0)
            {
                enemyShip = (EnemyShip)objs[0];
            }
        }
    }

    public class ShipStateSleeping : ShipStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemyShip.OnSleep();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            enemyShip.WakeUp();
        }
    }

    public class ShipStateShooting : ShipStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemyShip.StartShooting();
        }

        public override void OnStateStay()
        {
            base.OnStateStay();
            enemyShip.Aim();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            enemyShip.StopShooting();
        }
    }

    public class ShipStateSeeking : ShipStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemyShip.StartSeeking();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            enemyShip.StopSeeking();
        }
    }

    public class ShipStateDead : ShipStateBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemyShip.OnSleep();
            GameObject.Destroy(enemyShip.gameObject, 2f);
        }
    }
}