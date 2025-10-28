using HarmonyLib;

namespace MergeMyMOD
{
    public class DuckovSuperDuck
    {
        [HarmonyPatch(typeof(Health), "get_MaxHealth")]
        public class SuperHealthPower
        {
            [HarmonyPostfix]
            static void Postfix(Health __instance, ref float __result, CharacterMainControl ___characterCached)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (___characterCached.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.HealthPower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MaxEnergy")]
        public class SuperBasePower_MaxEnergy
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.BasePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MaxStamina")]
        public class SuperBasePower_MaxStamina
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.BasePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MaxWater")]
        public class SuperBasePower_MaxWater
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.BasePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_WaterEnergyRecoverMultiplier")]
        public class SuperBasePower_WaterEnergyRecoverMultiplier
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.BasePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MaxWeight")]
        public class SuperWeightPower
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.WeightPower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_CharacterMoveability")]
        public class SuperSpeedPower
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.SpeedPower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MeleeCritDamageGain")]
        public class SuperDamagePower_MeleeCritDamage
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.DamagePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MeleeCritRateGain")]
        public class SuperDamagePower_MeleeCritRate
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.DamagePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_MeleeDamageMultiplier")]
        public class SuperDamagePower_MeleeDamage
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.DamagePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_GunCritDamageGain")]
        public class SuperDamagePower_GunCritDamage
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.DamagePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_GunCritRateGain")]
        public class SuperDamagePower_GunCritRate
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.DamagePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_GunDamageMultiplier")]
        public class SuperDamagePower_GunDamage
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.DamagePower;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterMainControl), "get_StormProtection")]
        public class SuperProtectionPower
        {
            [HarmonyPostfix]
            static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    __result *= ModBehaviour.MyCustom.ProtectionPower;
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
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.IsMainCharacter)
                {
                    ___staminaRecoverTimer = 99999f;
                }
            }
        }

        //快速翻滚
        [HarmonyPatch(typeof(CA_Dash), nameof(CA_Dash.IsReady))]
        public class DashFast
        {
            [HarmonyPrefix]
            static void Prefix(CA_Dash __instance)
            {
                if (!ModBehaviour.MyCustom.isSuperDuck)
                {
                    return;
                }

                if (__instance.characterController.IsMainCharacter)
                {
                    __instance.coolTime = 0.1f;
                    __instance.staminaCost = 5f;
                }
            }
        }
    }
}