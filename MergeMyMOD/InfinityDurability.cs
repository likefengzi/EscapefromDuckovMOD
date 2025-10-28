using HarmonyLib;
using ItemStatsSystem;

namespace MergeMyMOD
{
    public class InfinityDurability
    {
        //不掉耐久上限
        [HarmonyPatch(typeof(Item), "get_DurabilityLoss")]
        public class NoMoreDurabilityLoss_Get
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isNoDurabilityLoss)
                {
                    return;
                }

                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }

            [HarmonyPostfix]
            static void Postfix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isNoDurabilityLoss)
                {
                    return;
                }

                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }
        }

        [HarmonyPatch(typeof(Item), "set_DurabilityLoss")]
        public class NoMoreDurabilityLoss_Set
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isNoDurabilityLoss)
                {
                    return;
                }

                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }

            [HarmonyPostfix]
            static void Postfix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isNoDurabilityLoss)
                {
                    return;
                }

                __instance.Variables.SetFloat("DurabilityLoss", 0, true);
            }
        }

        //回满耐久
        [HarmonyPatch(typeof(Item), "get_Durability")]
        public class MaxDurability_Get
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isInfinityDurability)
                {
                    return;
                }

                __instance.Variables.SetFloat("Durability", __instance.MaxDurability, true);
            }

            [HarmonyPostfix]
            static void Postfix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isInfinityDurability)
                {
                    return;
                }

                __instance.Variables.SetFloat("Durability", __instance.MaxDurability, true);
            }
        }

        [HarmonyPatch(typeof(Item), "set_Durability")]
        public class MaxDurability_Set
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isInfinityDurability)
                {
                    return;
                }

                __instance.Variables.SetFloat("Durability", __instance.MaxDurability, true);
            }

            [HarmonyPostfix]
            static void Postfix(Item __instance)
            {
                if (!ModBehaviour.MyCustom.isInfinityDurability)
                {
                    return;
                }

                __instance.Variables.SetFloat("Durability", __instance.MaxDurability, true);
            }
        }
    }
}