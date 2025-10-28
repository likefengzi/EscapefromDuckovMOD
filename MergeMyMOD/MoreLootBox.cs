using Duckov.Utilities;
using HarmonyLib;

namespace MergeMyMOD
{
    public class MoreLootBox
    {
        //每个点位都刷新
        [HarmonyPatch(typeof(LootBoxLoader), "RandomActive")]
        public class LootBoxIsAll
        {
            [HarmonyPostfix]
            static void Postfix(LootBoxLoader __instance)
            {
                if (!ModBehaviour.MyCustom.isMoreLootBox)
                {
                    return;
                }
                __instance.gameObject.SetActive(true);
            }
        }
    }
}