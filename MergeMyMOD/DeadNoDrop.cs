using System;
using System.Collections.Generic;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;

namespace MergeMyMOD
{
    public class DeadNoDrop
    {
        //死亡不掉落
        [HarmonyPatch(typeof(LevelManager), "OnMainCharacterDie")]
        public class NoDrop
        {
            [HarmonyPrefix]
            static void Prefix(LevelManager __instance, out List<Item> __state)
            {
                __state = new List<Item>();

                if (!ModBehaviour.MyCustom.isDeadNoDrop)
                {
                    return;
                }

                if (ModBehaviour.MyCustom.isSaveItem)
                {
                    foreach (Item item in __instance.MainCharacter.CharacterItem.Inventory)
                    {
                        __state.Add(item);
                    }


                    foreach (Item item in __state)
                    {
                        __instance.MainCharacter.CharacterItem.Inventory.RemoveItem(item);
                    }
                }


                try
                {
                    foreach (Slot slot in __instance.MainCharacter.CharacterItem.Slots)
                    {
                        try
                        {
                            if (slot.Content.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot))
                            {
                            }
                            else
                            {
                                slot.Content.Tags.Add(GameplayDataSettings.Tags.DontDropOnDeadInSlot);
                            }
                        }
                        catch (Exception e)
                        {
                            var a = e;
                        }
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                if (ModBehaviour.MyCustom.isSaveItem)
                {
                    try
                    {
                        foreach (Item item in __instance.MainCharacter.CharacterItem.Inventory)
                        {
                            try
                            {
                                if (item.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot))
                                {
                                }
                                else
                                {
                                    item.Tags.Add(GameplayDataSettings.Tags.DontDropOnDeadInSlot);
                                }
                            }
                            catch (Exception e)
                            {
                                var a = e;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var a = e;
                    }
                }
            }

            [HarmonyPostfix]
            static void Postfix(LevelManager __instance, List<Item> __state)
            {
                if (!ModBehaviour.MyCustom.isDeadNoDrop)
                {
                    return;
                }

                if (ModBehaviour.MyCustom.isSaveItem)
                {
                    foreach (Item item in __state)
                    {
                        __instance.MainCharacter.CharacterItem.Inventory.AddItem((Item)item);
                    }
                }
            }
        }
    }
}