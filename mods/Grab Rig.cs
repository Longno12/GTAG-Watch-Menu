using System;
using UnityEngine;

namespace Watch_Menu.mods
{
    internal class Grab_Rig
    {
        private static Vector3 initialRigPosition;
        private static bool isGrabbing = false;

        public static void GrabRig()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (!isGrabbing)
                {
                    isGrabbing = true;
                    initialRigPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                }
                Transform rightController = GorillaLocomotion.GTPlayer.Instance.GetControllerTransform(false);
                GorillaTagger.Instance.offlineVRRig.transform.position = rightController.position;
            }
            else if (isGrabbing)
            {
                isGrabbing = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = initialRigPosition;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
    }
}
