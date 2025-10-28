using HarmonyLib;
using UnityEngine;

namespace MergeMyMOD
{
    public class BetterFishing
    {
        [HarmonyPatch(typeof(Action_FishingV2), "OnUpdateAction")]
        public class BetterCatching
        {
            [HarmonyPrefix]
            static void Prefix(Action_FishingV2 __instance, ref float ___scaleTime, ref float ___waitTime)
            {
                if (!ModBehaviour.MyCustom.isBetterFishing)
                {
                    return;
                }
                __instance.successRange = new Vector2(0.001f, 999f);
                ___scaleTime = 999f;
                ___waitTime = 0.001f;
            }
        }
    }
}