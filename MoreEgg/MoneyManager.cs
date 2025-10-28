using System;
using System.Collections.Generic;
using Duckov.UI;
using HarmonyLib;
using ItemStatsSystem;
using NodeCanvas.Tasks.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoreEgg
{
    public class MoneyManager
    {
        // [HarmonyPatch(typeof(ItemDisplay), "get_CanUse")]
        // public class ItemCanUse
        // {
        //     [HarmonyPrefix]
        //     static bool Prefix(ItemDisplay __instance,ref bool __result)
        //     {
        //         try
        //         {
        //             if (__instance.Target.TypeID == 451 ||
        //                 __instance.Target.TypeID == 388 ||
        //                 (__instance.Target.TypeID >= 1097 && __instance.Target.TypeID <= 1126))
        //             {
        //                 __result = true;
        //                 return false;
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             var a = e;
        //         }
        //
        //         return true;
        //     }
        // }
        [HarmonyPatch(typeof(ItemHoveringUI), "SetupAndShow")]
        public class ItemDescription
        {
            [HarmonyPostfix]
            static void Postfix(ItemHoveringUI __instance, ItemDisplay display, TextMeshProUGUI ___itemDescription)
            {
                try
                {
                    if (display.Target.TypeID == 451)
                    {
                        ___itemDescription.text = display.Target.Description + "\n拆分现金，对应档位" +
                            "\n" +(int)(3000*ModBehaviour.aiMoneyMultiply)+
                            "\n" +(int)(9999*ModBehaviour.aiMoneyMultiply)+
                            "\n" +(int)(30000*ModBehaviour.aiMoneyMultiply)+
                            "\n" +(int)(50000*ModBehaviour.aiMoneyMultiply)+
                            "\n" +(int)(88888*ModBehaviour.aiMoneyMultiply)+
                            "\n右键使用，下单护航" ?? "";
                    }

                    if (display.Target.TypeID == 388)
                    {
                        ___itemDescription.text =
                            display.Target.Description + "\n0.2BTC，下单6套顶级绿护\n右键使用，下单护航" ?? "";
                    }

                    if (display.Target.TypeID >= 1097 && display.Target.TypeID <= 1126)
                    {
                        ___itemDescription.text = display.Target.Description + "\n1条鱼，下单体验陪玩\n右键使用，下单陪玩" ?? "";
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        [HarmonyPatch(typeof(ItemOperationMenu), "Setup")]
        public class ItemCanUseButton
        {
            [HarmonyPostfix]
            static void Postfix(ItemOperationMenu __instance, Button ___btn_Use, ItemDisplay ___TargetDisplay)
            {
                try
                {
                    if (___TargetDisplay.Target.TypeID == 451 ||
                        ___TargetDisplay.Target.TypeID == 388 ||
                        (___TargetDisplay.Target.TypeID >= 1097 && ___TargetDisplay.Target.TypeID <= 1126))
                    {
                        ___btn_Use.gameObject.SetActive(true);
                        ___btn_Use.interactable = true;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        [HarmonyPatch(typeof(ItemOperationMenu), "Use")]
        public class ItemUse
        {
            [HarmonyPrefix]
            static bool Prefix(ItemOperationMenu __instance, ItemDisplay ___TargetDisplay)
            {
                try
                {
                    if (___TargetDisplay.Target.TypeID == 451)
                    {
                        if (___TargetDisplay.Target.StackCount >= (int)(88888*ModBehaviour.aiMoneyMultiply))
                        {
                            ModBehaviour.Spawn(ModBehaviour.bossID,0.001f ,6, 5);
                            ___TargetDisplay.Target.StackCount -= (int)(88888*ModBehaviour.aiMoneyMultiply);
                            return false;
                        }

                        if (___TargetDisplay.Target.StackCount >= (int)(50000*ModBehaviour.aiMoneyMultiply))
                        {
                            ModBehaviour.Spawn(ModBehaviour.bossID,0.001f, 5, 4);
                            ___TargetDisplay.Target.StackCount -= (int)(50000*ModBehaviour.aiMoneyMultiply);
                            return false;
                        }

                        if (___TargetDisplay.Target.StackCount >= (int)(30000*ModBehaviour.aiMoneyMultiply))
                        {
                            ModBehaviour.Spawn(ModBehaviour.bossID,0.001f, 4, 4);
                            ___TargetDisplay.Target.StackCount -= (int)(30000*ModBehaviour.aiMoneyMultiply);
                            return false;
                        }

                        if (___TargetDisplay.Target.StackCount >= (int)(9999*ModBehaviour.aiMoneyMultiply))
                        {
                            ModBehaviour.Spawn(ModBehaviour.bossID,0.001f, 3, 3);
                            ___TargetDisplay.Target.StackCount -= (int)(9999*ModBehaviour.aiMoneyMultiply);
                            return false;
                        }

                        if (___TargetDisplay.Target.StackCount >= (int)(3000*ModBehaviour.aiMoneyMultiply))
                        {
                            ModBehaviour.Spawn(ModBehaviour.bossID,0.001f, 1, 2);
                            ___TargetDisplay.Target.StackCount -= (int)(3000*ModBehaviour.aiMoneyMultiply);
                            return false;
                        }

                        return false;
                    }

                    if (___TargetDisplay.Target.TypeID == 388)
                    {
                        ModBehaviour.Spawn(ModBehaviour.bossID,0.001f, 6, 6);
                        ___TargetDisplay.Target.DestroyTree();
                        return false;
                    }

                    if (___TargetDisplay.Target.TypeID >= 1097 &&
                        ___TargetDisplay.Target.TypeID <= 1126)
                    {
                        ModBehaviour.Spawn(ModBehaviour.bossID,0.001f, 0, 1,false);
                        ___TargetDisplay.Target.DestroyTree();
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
    }
}