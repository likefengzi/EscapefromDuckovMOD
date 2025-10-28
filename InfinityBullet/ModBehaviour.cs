using System.Reflection;
using HarmonyLib;
using ItemStatsSystem;

namespace InfinityBullet
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("InfinityBullet").PatchAll();
        }

        [HarmonyPatch(typeof(ItemAgent_Gun), "TransToFire")]
        public class NoBulletUsed
        {
            [HarmonyPrefix]
            static void Prefix(ItemAgent_Gun __instance)
            {
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