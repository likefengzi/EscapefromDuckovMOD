using System;
using System.Collections.Generic;
using Duckov.Rules;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;

namespace BetterEnemyBrain
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
            harmony.UnpatchAll("BetterEnemyBrain");
        }

        private void Start()
        {
            harmony=new Harmony("BetterEnemyBrain");
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(AIMainBrain), "Start")]
        public class BetterAIBrain
        {
            [HarmonyPostfix]
            static void Postfix(AIMainBrain __instance, ref Collider[] ___cols)
            {
                ___cols = new Collider[99999];
            }
        }
    }
}