using Oculus.Interaction.Input;
using UnityEngine;

namespace Watch_Menu.mods
{
    internal class NoClip
    {
        private static bool isNoClipActive = false;

        public static void NoClip1()
        {
            bool triggerHeldDown = (ControllerInputPoller.instance.rightControllerIndexFloat == 1f);
            if (triggerHeldDown != isNoClipActive)
            {
                isNoClipActive = triggerHeldDown;
                bool shouldCollidersBeEnabled = !isNoClipActive;
                foreach (MeshCollider worldCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    if (worldCollider != null)
                    {
                        worldCollider.enabled = shouldCollidersBeEnabled;
                    }
                }
            }
        }
    }
}
