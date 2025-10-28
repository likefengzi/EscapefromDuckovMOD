using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace NoFog
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }
        
        private void Start()
        {
            new Harmony("NoFog").PatchAll();
        }

        //去除迷雾
        [HarmonyPatch(typeof(FogOfWarManager), "Update")]
        public class NoFog
        {
            [HarmonyPrefix]
            static void Prefix(FogOfWarManager __instance, ref bool ___allVision)
            {
                // typeof(FogOfWarManager).GetField("allVision", BindingFlags.NonPublic | BindingFlags.Instance)?
                //     .SetValue(__instance, true);
                ___allVision = true;
            }
        }
    }
}