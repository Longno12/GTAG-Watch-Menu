using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace MenuPatch1
{
    public class MenuPatch : MonoBehaviour
    {
        public static bool IsPatched { get; private set; }

        internal static void ApplyHarmonyPatches()
        {
            if (!MenuPatch.IsPatched)
            {
                if (MenuPatch.instance == null)
                {
                    MenuPatch.instance = new Harmony("com.encryptic.gorillatag.watchmenu");
                }
                MenuPatch.instance.PatchAll(Assembly.GetExecutingAssembly());
                MenuPatch.IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (MenuPatch.instance != null && MenuPatch.IsPatched)
            {
                MenuPatch.IsPatched = true;
            }
        }

        private static Harmony instance;

        public const string InstanceId = "com.encryptic.gorillatag.watchmenu";
    }
}
