using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace encryptic.Patches.Remade
{
    [HarmonyPatch]
    public static class LocalRigLifecyclePatch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(VRRig), "Awake");
            yield return AccessTools.Method(typeof(VRRig), "OnDisable");
        }

        private static bool Prefix(VRRig __instance)
        {
            bool isLocalRig = __instance.isLocal || __instance.gameObject.name == "Local Gorilla Player(Clone)";

            return !isLocalRig;
        }
    }
}