using System;
using System.Reflection;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;

namespace HighQualityItem
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("HighQualityItem").PatchAll();
        }

        private void Update()
        {
            return;
            try
            {
                typeof(LevelConfig)
                    .GetField("lootBoxHighQualityChanceMultiplier", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(LevelConfig.Instance, 9f);
                typeof(LevelConfig)
                    .GetField("lootboxItemCountMultiplier", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(LevelConfig.Instance, 10);
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        //快速翻滚
        [HarmonyPatch(typeof(RandomContainer<int>), nameof(RandomContainer<int>.GetRandom),
            new Type[] { typeof(float) })]
        public class BestItem
        {
            [HarmonyPrefix]
            static bool Prefix(RandomContainer<int> __instance,ref int __result)
            {
                float maxWeight = 0;
                RandomContainer<int>.Entry maxEntry=__instance.entries[0];
                foreach (RandomContainer<int>.Entry entry in __instance.entries)
                {
                    if (entry.weight>=maxWeight)
                    {
                        maxWeight = entry.weight;
                        maxEntry = entry;
                    }
                }
                maxEntry=__instance.entries[__instance.entries.Count-1];
                __result = maxEntry.value;
                typeof(LevelConfig)
                    .GetField("lootBoxHighQualityChanceMultiplier", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(LevelConfig.Instance, 9f);
                typeof(LevelConfig)
                    .GetField("lootboxItemCountMultiplier", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(LevelConfig.Instance, 10);
                return false;
            }
        }
    }
}