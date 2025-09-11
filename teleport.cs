using HarmonyLib;
using Oculus.Platform.Models;
using UnityEngine;

namespace Encryptic.Patches
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer), "LateUpdate", MethodType.Normal)]
    internal class TeleportPatch
    {
        public static bool doTeleport = false;
        public static Vector3 telePos;
        private static bool teleportOnce;
        private static Vector3 destination;
        private static bool teleporting;
        public static bool Prefix(GorillaLocomotion.GTPlayer __instance, ref Vector3 ___lastPosition, ref Vector3 ___lastHeadPosition, ref Vector3 ___lastLeftHandPosition, ref Vector3 ___lastRightHandPosition, ref Vector3[] ___velocityHistory, ref Vector3 ___currentVelocity)
        {
            if (doTeleport)
            {
                Rigidbody playerRb = __instance.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.transform.position = World2Player(telePos);
                    ___lastPosition = telePos;
                    ___lastHeadPosition = __instance.headCollider.transform.position;
                    ___lastLeftHandPosition = telePos;
                    ___lastRightHandPosition = telePos;
                    ___velocityHistory = new Vector3[__instance.velocityHistorySize];
                    ___currentVelocity = playerRb.linearVelocity;
                }
                doTeleport = false;
            }
            return true;
        }

        public static Vector3 World2Player(Vector3 worldPosition)
        {
            return worldPosition - GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.transform.position;
        }

        internal static void Teleport(Vector3 TeleportDestination)
        {
            teleporting = true;
            destination = TeleportDestination;
        }

        internal static void TeleportOnce(Vector3 TeleportDestination, bool stateDepender)
        {
            if (stateDepender)
            {
                if (!teleportOnce)
                {
                    teleporting = true;
                    destination = TeleportDestination;
                }
                teleportOnce = true;
            }
            else
            {
                teleportOnce = false;
            }
        }
    }
}
