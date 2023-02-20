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
        public GameObject shot_vfx;
        public float cooldown = 2f;
        public LayerMask hitLayer;
        public float damage;

        private bool _onCooldown;
        private CannonBallBase _cannonBall;

        public void Shoot(IKiller shooter = null)
        {
            if(!_onCooldown)
            {
                if(shot_vfx != null)
                {
                    Instantiate(shot_vfx, transform.position, transform.rotation);
                }
                _cannonBall = CannonBallPooling.Instance.GetPoolItem();

                _cannonBall.Shoot(transform, hitLayer, shooter, damage);

                _onCooldown = true;
                StartCoroutine(StartCooldown());
            }
        }

        public bool IsOnCooldown()
        {
            return _onCooldown;
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(cooldown);
            _onCooldown = false;
        }
    }
}