using HarmonyLib;
using ItemStatsSystem;

namespace MergeMyMOD
{
    public class InfinityBullet
    {
        [HarmonyPatch(typeof(ItemAgent_Gun), "TransToFire")]
        public class NoBulletUsed
        {
            [HarmonyPrefix]
            static void Prefix(ItemAgent_Gun __instance)
            {
                if (!ModBehaviour.MyCustom.isInfinityBullet)
                {
                    return;
                }

                if (__instance.Holder.IsMainCharacter)
                {
                    foreach (Item item in __instance.GunItemSetting.Item.Inventory)
                    {
                        if (!(item == null) && item.StackCount >= 1)
                        {
                            item.StackCount++;
                            break;
                        }
                    }
                }
            }
        }
    }
}