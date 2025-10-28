using System;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace StrongerEnemy
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public Harmony harmony;
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void OnDisable()
        {
            harmony.UnpatchAll("StrongerEnemy");
        }

        private void Start()
        {
            harmony = new Harmony("StrongerEnemy");
            harmony.PatchAll();
        }

        //千里眼顺风耳
        [HarmonyPatch(typeof(AIMainBrain), "DoSearch")]
        public class EnemyIsBlind
        {
            [HarmonyPrefix]
            static bool Prefix(AIMainBrain __instance, ref AIMainBrain.SearchTaskContext context,CharacterMainControl ____mc)
            {
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
                ___distance = 1000000;
            }
        }

        //每个点位都刷新敌人
        // [HarmonyPatch(typeof(RandomCharacterSpawner), "CreateAsync")]
        // public class EnemyIsAll
        // {
        //     [HarmonyPrefix]
        //     static void Prefix(RandomCharacterSpawner __instance)
        //     {
        //         __instance.spawnCountRange =
        //             new Vector2Int(__instance.spawnPoints.points.Count, __instance.spawnPoints.points.Count);
        //     }
        // }
    }
}