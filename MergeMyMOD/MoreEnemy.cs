using System;
using System.Collections.Generic;
using Duckov.Rules;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;

namespace MergeMyMOD
{
    public class MoreEnemy
    {
        //每个点位都刷新敌人
        [HarmonyPatch(typeof(RandomCharacterSpawner), "CreateAsync")]
        public class EnemyIsAll
        {
            [HarmonyPrefix]
            static void Prefix(RandomCharacterSpawner __instance)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return;
                }

                if (ModBehaviour.MyCustom.isMorePoints)
                {
                    __instance.spawnCountRange =
                        new Vector2Int(__instance.spawnPoints.points.Count,
                            __instance.spawnPoints.points.Count);
                }
            }
        }

        //更多敌人
        [HarmonyPatch(typeof(RandomCharacterSpawner), nameof(RandomCharacterSpawner.StartSpawn))]
        public class EnemyIsMore
        {
            public static int num;

            [HarmonyPrefix]
            static bool Prefix(RandomCharacterSpawner __instance)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return true;
                }

                if (num > 0)
                {
                    num--;
                    return true;
                }

                if (__instance.masterGroup && !__instance.masterGroup.hasLeader)
                {
                    num = ModBehaviour.MyCustom.BossMultiply;
                }
                else
                {
                    num = ModBehaviour.MyCustom.EnemyMultiply;
                }

                return false;
            }

            [HarmonyPostfix]
            static void Postfix(RandomCharacterSpawner __instance)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return;
                }

                if (num > 0)
                {
                    __instance.StartSpawn();
                }
            }
        }

        //千里眼顺风耳
        [HarmonyPatch(typeof(AIMainBrain), "DoSearch")]
        public class EnemyIsBlind
        {
            [HarmonyPrefix]
            static bool Prefix(AIMainBrain __instance, ref AIMainBrain.SearchTaskContext context,
                CharacterMainControl ____mc)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return true;
                }

                if (!ModBehaviour.MyCustom.isStrongerEnemy)
                {
                    return true;
                }

                try
                {
                    if (____mc.GetComponent<AICharacterController>().leader.IsMainCharacter)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                try
                {
                    if (Team.IsEnemy(context.selfTeam, LevelManager.Instance.MainCharacter.Team))
                    {
                        context.onSearchFinishedCallback(LevelManager.Instance.MainCharacter.mainDamageReceiver, null);
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return true;
            }
        }

        //全场激活
        [HarmonyPatch(typeof(SetActiveByPlayerDistance), "FixedUpdate")]
        public class EnemyIsActive
        {
            [HarmonyPrefix]
            static void Prefix(SetActiveByPlayerDistance __instance, ref float ___distance)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return;
                }

                if (!ModBehaviour.MyCustom.isStrongerEnemy)
                {
                    return;
                }

                ___distance = 1000000;
            }
        }

        [HarmonyPatch(typeof(CharacterRandomPreset), "AddBullet")]
        public class BetterEnemy_BetterBullet
        {
            [HarmonyPrefix]
            static void Prefix(CharacterRandomPreset __instance, ref RandomContainer<int> ___bulletQualityDistribution)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return;
                }

                if (!ModBehaviour.MyCustom.isBetterEnemy)
                {
                    return;
                }

                List<RandomContainer<int>.Entry> list = new List<RandomContainer<int>.Entry>();
                for (int i = 0; i < ___bulletQualityDistribution.entries.Count; i++)
                {
                    ___bulletQualityDistribution.entries[i] =
                        ___bulletQualityDistribution.entries[___bulletQualityDistribution.entries.Count - 1];
                }
            }
        }

        [HarmonyPatch(typeof(RandomItemGenerateDescription), "Generate")]
        public class BetterEnemy_BetterItem
        {
            [HarmonyPrefix]
            static void Prefix(RandomItemGenerateDescription __instance)
            {
                if (!ModBehaviour.MyCustom.isMoreEnemy)
                {
                    return;
                }

                if (!ModBehaviour.MyCustom.isBetterEnemy)
                {
                    return;
                }

                List<RandomContainer<int>.Entry> list = new List<RandomContainer<int>.Entry>();
                for (int i = 0; i < __instance.qualities.entries.Count; i++)
                {
                    __instance.qualities.entries[i] =
                        __instance.qualities.entries[__instance.qualities.entries.Count - 1];
                }
            }
        }

        // [HarmonyPatch(typeof(Ruleset), "get_EnemyAttackTimeFactor")]
        // public class BetterEnemy1
        // {
        //     [HarmonyPostfix]
        //     static void Postfix(Ruleset __instance, ref float __result)
        //     {
        //         if (!ModBehaviour.MyCustom.isMoreEnemy)
        //         {
        //             return;
        //         }
        //
        //         if (!ModBehaviour.MyCustom.isBetterEnemy)
        //         {
        //             return;
        //         }
        //
        //         __result *= 9;
        //     }
        // }
        //
        // [HarmonyPatch(typeof(Ruleset), "get_EnemyAttackTimeSpaceFactor")]
        // public class BetterEnemy2
        // {
        //     [HarmonyPostfix]
        //     static void Postfix(Ruleset __instance, ref float __result)
        //     {
        //         if (!ModBehaviour.MyCustom.isMoreEnemy)
        //         {
        //             return;
        //         }
        //
        //         if (!ModBehaviour.MyCustom.isBetterEnemy)
        //         {
        //             return;
        //         }
        //
        //         __result /= 9;
        //     }
        // }
        //
        // [HarmonyPatch(typeof(Ruleset), "get_EnemyReactionTimeFactor")]
        // public class BetterEnemy3
        // {
        //     [HarmonyPostfix]
        //     static void Postfix(Ruleset __instance, ref float __result)
        //     {
        //         if (!ModBehaviour.MyCustom.isMoreEnemy)
        //         {
        //             return;
        //         }
        //
        //         if (!ModBehaviour.MyCustom.isBetterEnemy)
        //         {
        //             return;
        //         }
        //
        //         __result /= 9;
        //     }
        // }
    }
}