using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Duckov.Rules;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;

namespace BetterEnemy
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("BetterEnemy").PatchAll();
            
        }

        
        [HarmonyPatch(typeof(Ruleset), "get_EnemyAttackTimeFactor")]
        public class BetterEnemy1
        {
            [HarmonyPostfix]
            static void Postfix(Ruleset __instance,ref float __result)
            {
                __result *= 9;
            }
        }
        [HarmonyPatch(typeof(Ruleset), "get_EnemyAttackTimeSpaceFactor")]
        public class BetterEnemy2
        {
            [HarmonyPostfix]
            static void Postfix(Ruleset __instance,ref float __result)
            {
                __result /= 9;
            }
        }
        [HarmonyPatch(typeof(Ruleset), "get_EnemyHealthFactor")]
        public class BetterEnemy3
        {
            [HarmonyPostfix]
            static void Postfix(Ruleset __instance,ref float __result)
            {
                //__result *= 9;
            }
        }
        [HarmonyPatch(typeof(Ruleset), "get_EnemyReactionTimeFactor")]
        public class BetterEnemy4
        {
            [HarmonyPostfix]
            static void Postfix(Ruleset __instance,ref float __result)
            {
                __result /= 9;
            }
        }
        // [HarmonyPatch(typeof(RandomContainer<int>), nameof(RandomContainer<int>.GetRandom),new []{typeof(int)})]
        // public class BetterEnemy5
        // {
        //     [HarmonyPrefix]
        //     static void Prefix(RandomContainer<int> __instance,ref float lowPercent)
        //     {
        //         lowPercent = 999f;
        //     }
        // }
        [HarmonyPatch(typeof(CharacterRandomPreset), "AddBullet")]
        public class BetterEnemy5
        {
            [HarmonyPrefix]
            static void Prefix(CharacterRandomPreset __instance,ref RandomContainer<int> ___bulletQualityDistribution)
            {
                List<RandomContainer<int>.Entry> list =new List<RandomContainer<int>.Entry>();
                for (int i = 0; i < ___bulletQualityDistribution.entries.Count; i++)
                {
                    ___bulletQualityDistribution.entries[i] =
                        ___bulletQualityDistribution.entries[___bulletQualityDistribution.entries.Count - 1];
                }
            }
        }
        [HarmonyPatch(typeof(RandomItemGenerateDescription), "Generate")]
        public class BetterEnemy6
        {
            [HarmonyPrefix]
            static void Prefix(RandomItemGenerateDescription __instance)
            {
                List<RandomContainer<int>.Entry> list =new List<RandomContainer<int>.Entry>();
                for (int i = 0; i < __instance.qualities.entries.Count; i++)
                {
                    __instance.qualities.entries[i] =
                        __instance.qualities.entries[__instance.qualities.entries.Count - 1];
                }
            }
        }
        
        
        
        // if (ItemAssetsCollection.TryGetDynamicEntry(typeID, out dynamicEntry))
        // {
        //     result = UnityEngine.Object.Instantiate<Item>(dynamicEntry.prefab);
        // }
    }
}