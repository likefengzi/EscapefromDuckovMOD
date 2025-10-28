using System.Reflection;
using Duckov.UI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MergeMyMOD
{
    public class SuperPet
    {
        //更多安全箱
        [HarmonyPatch(typeof(CharacterMainControl), "get_PetCapcity")]
        public class MorePetCapcity
        {
            [HarmonyPrefix]
            static bool Prefix(CharacterMainControl __instance, ref int __result)
            {
                if (!ModBehaviour.MyCustom.isSuperPet)
                {
                    return true;
                }

                if (__instance.IsMainCharacter)
                {
                    if (ModBehaviour.MyCustom.isSuperPet77)
                    {
                        __result = 7 * 7;
                    }
                    else
                    {
                        __result = 3 * 3;
                    }

                    return false;
                }

                return true;
            }
        }

        //更多安全箱
        [HarmonyPatch(typeof(LootView), "OnOpen")]
        public class MorePetCapcity33
        {
            [HarmonyPrefix]
            static void Prefix(LootView __instance, InventoryDisplay ___petInventoryDisplay)
            {
                if (!ModBehaviour.MyCustom.isSuperPet)
                {
                    return;
                }

                if (PetProxy.PetInventory)
                {
                    FieldInfo contentLayoutField = typeof(InventoryDisplay).GetField(
                        "contentLayout",
                        BindingFlags.NonPublic | BindingFlags.Instance
                    );
                    if (contentLayoutField != null)
                    {
                        GridLayoutGroup contentLayout =
                            (GridLayoutGroup)contentLayoutField.GetValue(___petInventoryDisplay);

                        // 3. 修改 GridLayoutGroup 属性
                        contentLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                        if (ModBehaviour.MyCustom.isSuperPet77)
                        {
                            contentLayout.constraintCount = 7;
                            contentLayout.padding =
                                new RectOffset((int)(contentLayout.cellSize.x * 7 / 2f * 1.6), 0,
                                    (int)(contentLayout.cellSize.y / 2), 0);
                        }
                        else
                        {
                            contentLayout.constraintCount = 3;
                            contentLayout.padding =
                                new RectOffset((int)(contentLayout.cellSize.x * 3 / 2f), 0,
                                    (int)(contentLayout.cellSize.y / 2), 0);
                        }


                        //contentLayout.cellSize = new Vector2(100f, 100f); // 调整单元格大小
                        //contentLayout.spacing = new Vector2(10f, 10f);    // 调整间距

                        FieldInfo gridLayoutElementField = typeof(InventoryDisplay).GetField(
                            "gridLayoutElement",
                            BindingFlags.NonPublic | BindingFlags.Instance
                        );
                        if (gridLayoutElementField != null)
                        {
                            LayoutElement gridLayoutElement =
                                (LayoutElement)gridLayoutElementField.GetValue(___petInventoryDisplay);
                            //gridLayoutElement.minWidth = 99 * contentLayout.cellSize.x;
                            gridLayoutElement.ignoreLayout = true;
                        }
                    }
                }
            }
        }
    }
}