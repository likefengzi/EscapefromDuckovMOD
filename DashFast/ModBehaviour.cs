using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ECM2.Walkthrough.Ex43;
using HarmonyLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;

namespace DashFast
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static float speedMultiply;

        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("DashFast").PatchAll();
            ModBehaviour.speedMultiply = LoadData.LoadDataFromFile(2f);
        }

        //快速步行
        [HarmonyPatch(typeof(CharacterMainControl), "get_CharacterWalkSpeed")]
        public class WalkFast
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.speedMultiply;
                }
            }
        }

        //快速冲刺
        [HarmonyPatch(typeof(Movement), "get_runSpeed")]
        public class RunFast
        {
            [HarmonyPrefix]
            static bool Prefix(Movement __instance, ref float __result)
            {
                if (__instance.characterController.IsMainCharacter)
                {
                    __result = __instance.characterController.CharacterRunSpeed * ModBehaviour.speedMultiply;
                    return false;
                }

                return true;
            }
        }

        //快速翻滚
        [HarmonyPatch(typeof(CA_Dash), nameof(CA_Dash.IsReady))]
        public class DashFast
        {
            [HarmonyPrefix]
            static void Prefix(CA_Dash __instance)
            {
                if (__instance.characterController.IsMainCharacter)
                {
                    __instance.coolTime = 0.1f;
                    __instance.staminaCost = 5f;
                }
            }
        }

        //快速恢复体力
        [HarmonyPatch(typeof(CharacterMainControl), "UpdateStats")]
        public class RecoverFast
        {
            [HarmonyPrefix]
            static void Prefix(CharacterMainControl __instance, ref float ___staminaRecoverTimer)
            {
                if (__instance.IsMainCharacter)
                {
                    ___staminaRecoverTimer = 99999f;
                }
            }
        }
    }
}