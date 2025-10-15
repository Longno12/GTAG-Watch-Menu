using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Watch_Menu.mods
{
    internal class Size_Changer
    {
        public static void SizeChanger()
        {
            float sizeScale = GorillaTagger.Instance.offlineVRRig.NativeScale;
            float increment = 0.1f;
            if (ControllerInputPoller.instance.leftGrab)
            {
                sizeScale -= increment;
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                sizeScale += increment;
            }
            if (sizeScale < 0.05f)
            {
                sizeScale = 0.05f;
            }
            GorillaTagger.Instance.offlineVRRig.transform.localScale = Vector3.one * sizeScale;
            GorillaTagger.Instance.offlineVRRig.NativeScale = sizeScale;
            typeof(GorillaLocomotion.GTPlayer).GetField("nativeScale", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(GorillaLocomotion.GTPlayer.Instance, sizeScale);
        }
    }
}
