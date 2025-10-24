using UnityEngine;
using System.Reflection;

namespace Watch_Menu.mods
{
    internal class Size_Changer
    {
        public static void SizeChanger()
        {
            float scaleChange = 0.1f;
            float minScale = 0.05f;
            var rig = GorillaTagger.Instance.offlineVRRig;
            float currentScale = rig.NativeScale;
            if (ControllerInputPoller.instance.leftGrab)
            {
                currentScale -= scaleChange;
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                currentScale += scaleChange;
            }
            if (currentScale < minScale)
            {
                currentScale = minScale;
            }
            rig.transform.localScale = Vector3.one * currentScale;
            rig.NativeScale = currentScale;
            var playerType = typeof(GorillaLocomotion.GTPlayer);
            var scaleField = playerType.GetField("nativeScale", BindingFlags.NonPublic | BindingFlags.Instance);
            if (scaleField != null)
            {
                scaleField.SetValue(GorillaLocomotion.GTPlayer.Instance, currentScale);
            }
        }
    }
}
