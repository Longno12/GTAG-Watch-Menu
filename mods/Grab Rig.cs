using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static QRCoder.PayloadGenerator;

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
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
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
