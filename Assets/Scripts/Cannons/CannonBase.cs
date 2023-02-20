using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cannon
{
    public enum CannonSide
    {
        FRONT,
        LEFT,
        RIGHT,
        BACK
    }

    public class CannonBase : MonoBehaviour
    {
        public CannonSide side;
        public CannonBallBase cannonBall_pfb;
        public GameObject shoot_vfx;
        public float cooldown = 2f;

        private bool _onCooldown;
        private CannonBallBase _cannonBall;

        public void Shoot(IKiller shooter = null)
        {
            if(!_onCooldown)
            {
                if(shoot_vfx != null)
                {
                    Instantiate(shoot_vfx, transform.position, transform.rotation);
                }
                _cannonBall = Instantiate(cannonBall_pfb, transform.position, transform.rotation);
                _cannonBall.shooter = shooter;
                _onCooldown = true;
                StartCoroutine(StartCooldown());
            }
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(cooldown);
            _onCooldown = false;
        }
    }
}