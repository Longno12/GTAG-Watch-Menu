using System.ComponentModel;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using MenuPatch1;

namespace HarmonyPatch1
{
    [Description("encryptic watch")]
    [BepInPlugin("com.longno.gorillatag.encryptic_watch", "Encryptic Watch Menu", "1.0.0")]

    public class HarmonyPatch : BaseUnityPlugin
    {
        public static bool modmenupatch = true;

        private void OnEnable()
        {
            MenuPatch.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            MenuPatch.RemoveHarmonyPatches();
        }
    }
}
