using HarmonyLib;
using ItemStatsSystem;

namespace NoDurabilityLoss
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("NoDurabilityLoss").PatchAll();
        }

        //不掉耐久上限
        [HarmonyPatch(typeof(Item), "get_DurabilityLoss")]
        public class NoMoreDurabilityLoss_Get
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance)
            {
                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }

            [HarmonyPostfix]
            static void Postfix(Item __instance)
            {
                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }
        }
        [HarmonyPatch(typeof(Item), "set_DurabilityLoss")]
        public class NoMoreDurabilityLoss_Set
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance)
            {
                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }

            [HarmonyPostfix]
            static void Postfix(Item __instance)
            {
                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }
        }
    }
}