using System;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;

namespace MoreLootBox
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
            harmony.UnpatchAll("MoreLootBox");
        }

        private void Start()
        {
            harmony=new Harmony("MoreLootBox");
            harmony.PatchAll();
        }

        //每个点位都刷新
        [HarmonyPatch(typeof(LootBoxLoader), "RandomActive")]
        public class LootBoxIsAll
        {
            [HarmonyPostfix]
            static void Postfix(LootBoxLoader __instance)
            {
                __instance.gameObject.SetActive(true);
            }
        }
    }
}