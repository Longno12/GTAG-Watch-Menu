using System;
using HarmonyLib;
using UnityEngine;

namespace ShaderFix1
{
    [HarmonyPatch(typeof(GameObject))]
    [HarmonyPatch("CreatePrimitive", 0)]
    internal class ShaderFix : MonoBehaviour
    {
        private static void Postfix(GameObject __result)
        {
            __result.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
            __result.GetComponent<Renderer>().material.color = new Color32(30, 12, 73, 1);
        }
    }
}