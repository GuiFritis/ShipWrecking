using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;

namespace Enemy
{
    [RequireComponent(typeof(ShipBase))]
    public abstract class EnemyShip : EnemyBase
    {
        public abstract void WakeUp();
        public abstract void Sleep();
    }   
}