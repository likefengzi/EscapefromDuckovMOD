using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Fishing;
using Fishing.UI;
using HarmonyLib;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BetterFishing
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static Dictionary<string, float> fishingData = new Dictionary<string, float>();

        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("BetterFishing").PatchAll();
            ModBehaviour.fishingData = LoadData.LoadDataFromFile();
        }

        //自动钓鱼
        // [HarmonyPatch(typeof(Action_FishingV2), "SpawnFish")]
        // [HarmonyPatch(typeof(Action_FishingV2), "TransToWaiting")]
        // [HarmonyPatch(typeof(Action_FishingV2), "TryCatch")]
        // public class BetterCatching
        // {
        //     [HarmonyPostfix]
        //     static void Postfix(Action_FishingV2 __instance, Item ___currentFish)
        //     {
        //         try
        //         {
        //             if (___currentFish != null)
        //             {
        //                 ItemUtilities.SendToPlayer(___currentFish, false, false);
        //                 ItemUtilities.SendToPlayer(___currentFish, true, false);
        //                 ItemUtilities.SendToPlayer(___currentFish, true, false);
        //                 ItemUtilities.SendToPlayer(___currentFish, true, true);
        //                 LevelManager.Instance.MainCharacter.CharacterItem.Inventory.AddItem(___currentFish);
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             var a = e;
        //         }
        //
        //         __instance.successRange = new Vector2(0.001f, 99999f);
        //     }
        // }

        //自动钓鱼
        // [HarmonyPatch(typeof(Action_FishingV2), "SpawnFish")]
        // [HarmonyPatch(typeof(Action_FishingV2), "TransToWaiting")]
        // [HarmonyPatch(typeof(Action_FishingV2), "TryCatch")]
        [HarmonyPatch(typeof(Action_FishingV2), "OnUpdateAction")]
        // [HarmonyPatch(typeof(Action_FishingV2), "SpawnBucketParticle")]
        // [HarmonyPatch(typeof(Action_FishingV2), "SpawnDropParticle")]
        // [HarmonyPatch(typeof(Action_FishingV2), "SetWaveEmissionRate")]
        // [HarmonyPatch(typeof(Action_FishingV2), "OnStart")]
        // [HarmonyPatch(typeof(Action_FishingV2), "IsReady")]
        // [HarmonyPatch(typeof(Action_FishingV2), "CanUseHand")]
        // [HarmonyPatch(typeof(Action_FishingV2), "CanRun")]
        // [HarmonyPatch(typeof(Action_FishingV2), "CanMove")]
        // [HarmonyPatch(typeof(Action_FishingV2), "CanEditInventory")]
        // [HarmonyPatch(typeof(Action_FishingV2), "CanControlAim")]
        public class BetterCatching3
        {
            [HarmonyPrefix]
            static void Prefix(Action_FishingV2 __instance, ref float ___scaleTime, ref float ___waitTime)
            {
                __instance.successRange = new Vector2(
                    ModBehaviour.fishingData["successRangeX"],
                    ModBehaviour.fishingData["successRangeY"]
                );
                ___scaleTime = ModBehaviour.fishingData["scaleTime"];
                ___waitTime = ModBehaviour.fishingData["waitTime"];
            }
        }

        //自动钓鱼
        // [HarmonyPatch(typeof(Action_Fishing), "WaitForSelectBait")]
        // [HarmonyPatch(typeof(Action_Fishing), "SingleFishingLoop")]
        // [HarmonyPatch(typeof(Action_Fishing), "SelectBaitAndStartFishing")]
        // [HarmonyPatch(typeof(Action_Fishing), "GetAllBaits")]
        // [HarmonyPatch(typeof(Action_Fishing), "Fishing")]
        // [HarmonyPatch(typeof(Action_Fishing), "CanUseHand")]
        // [HarmonyPatch(typeof(Action_Fishing), "CanRun")]
        // [HarmonyPatch(typeof(Action_Fishing), "CanMove")]
        // [HarmonyPatch(typeof(Action_Fishing), "CanEditInventory")]
        // [HarmonyPatch(typeof(Action_Fishing), "CanControlAim")]
        // public class BetterCatching2
        // {
        //     [HarmonyPrefix]
        //     static void Prefix(Action_Fishing __instance, ref float ___catchTime)
        //     {
        //         ___catchTime = 10f;
        //     }
        // }


        // private void Update()
        // {
        //     if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //     {
        //         this.TryAddFish();
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         this.TryAddFish();
        //     }
        // }

        // public void TryAddFish()
        // {
        //     try
        //     {
        //         // 获取角色控制对象
        //         CharacterMainControl characterMainControl = LevelManager.Instance.MainCharacter;
        //
        //         // 1. 获取 `currentAction` 字段（假设它是 private 的）
        //         FieldInfo currentActionField = typeof(CharacterMainControl).GetField(
        //             "currentAction",
        //             BindingFlags.NonPublic | BindingFlags.Instance // 搜索私有实例字段
        //         );
        //
        //         if (currentActionField != null)
        //         {
        //             // 读取 `currentAction` 的值
        //             object currentActionValue = currentActionField.GetValue(characterMainControl);
        //
        //             // 2. 检查是否是 Action_FishingV2 类型
        //             if (currentActionValue is Action_FishingV2 action_FishingV)
        //             {
        //                 // 3. 获取 `currentFish` 字段（假设它是 private 的）
        //                 FieldInfo currentFishField = typeof(Action_FishingV2).GetField(
        //                     "currentFish",
        //                     BindingFlags.NonPublic | BindingFlags.Instance
        //                 );
        //
        //                 if (currentFishField != null)
        //                 {
        //                     // 读取 `currentFish` 的值
        //                     Item currentFish = (Item)currentFishField.GetValue(action_FishingV);
        //                     ItemUtilities.SendToPlayer(currentFish, false, false);
        //                     ItemUtilities.SendToPlayer(currentFish, true, false);
        //                     ItemUtilities.SendToPlayer(currentFish, true, false);
        //                     ItemUtilities.SendToPlayer(currentFish, true, true);
        //                     characterMainControl.CharacterItem.Inventory.AddItem(currentFish);
        //                 }
        //                 else
        //                 {
        //                 }
        //             }
        //             else
        //             {
        //             }
        //         }
        //         else
        //         {
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         var a = e;
        //     }
        // }
    }
}