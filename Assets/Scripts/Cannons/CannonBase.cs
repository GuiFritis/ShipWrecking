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
        public GameObject cannonBall_pfb;
        public float cooldown = 2f;

        private bool _onCooldown;

        public void Shoot()
        {
            if(!_onCooldown)
            {
                Instantiate(cannonBall_pfb, transform.position, transform.rotation);
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