using HarmonyLib;
using ItemStatsSystem;

namespace MergeMyMOD
{
    public class HighQualityItem
    {
        [HarmonyPatch(typeof(LevelConfig), "get_LootBoxQualityLowPercent")]
        public class LootBoxQualityLowPercent
        {
            [HarmonyPrefix]
            static bool Prefix(Item __instance,ref float __result)
            {
                if (!ModBehaviour.MyCustom.isHighQualityItem)
                {
                    return true;
                }

                __result = ModBehaviour.MyCustom.HighQualityChanceMultiplier;
                return false;
            }
        }
        [HarmonyPatch(typeof(LevelConfig), "get_LootboxItemCountMultiplier")]
        public class ItemCountMultiplier
        {
            [HarmonyPrefix]
            static bool Prefix(Item __instance,ref float __result)
            {
                if (!ModBehaviour.MyCustom.isHighQualityItem)
                {
                    return true;
                }

                __result = ModBehaviour.MyCustom.ItemCountMultiplier;
                return false;
            }
        }
    }
}