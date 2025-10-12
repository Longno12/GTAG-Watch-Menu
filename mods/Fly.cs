using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Watch_Menu.mods
{
    internal class Fly
    {
        public static void Fly1()
        {
            float controllerIndexFloat = ControllerInputPoller.instance.rightControllerIndexFloat;
            if (controllerIndexFloat == 1f && Mathf.Approximately(controllerIndexFloat, 1.0f))
            {
                GorillaLocomotion.GTPlayer PlayerInstance = GorillaLocomotion.GTPlayer.Instance;
                if (PlayerInstance != null)
                {
                    UnityEngine.Transform PlayerTransform = PlayerInstance.transform;
                    UnityEngine.Collider headCollider = PlayerInstance.headCollider;
                    if (headCollider != null && PlayerTransform != null)
                    {
                        UnityEngine.Vector3 headForwardDirection = headCollider.transform.forward;
                        UnityEngine.Vector3 movementVector = headForwardDirection * Time.deltaTime;
                        movementVector *= flySpeed12;
                        UnityEngine.Vector3 newPosition = PlayerTransform.position + movementVector;
                        PlayerTransform.position = newPosition;
                        UnityEngine.Rigidbody PlayerRigidbody = PlayerInstance.GetComponent<UnityEngine.Rigidbody>();
                        if (PlayerRigidbody != null)
                        {
                            UnityEngine.Vector3 zeroVelocity = UnityEngine.Vector3.zero;
                            PlayerRigidbody.linearVelocity = zeroVelocity;
                        }
                    }
                }
            }
        }
        public static float flySpeed12 = 10f;
    }
}
