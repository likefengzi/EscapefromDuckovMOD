using Duckov.BlackMarkets;
using HarmonyLib;

namespace BetterBlackMarket
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }
        
        private void Start()
        {
            new Harmony("BetterBlackMarket").PatchAll();
        }

        //无限刷新次数
        [HarmonyPatch(typeof(BlackMarket), nameof(BlackMarket.PayAndRegenerate))]
        public class MoreRefresh
        {
            [HarmonyPrefix]
            static void Prefix(BlackMarket __instance)
            {
                __instance.RefreshChance = __instance.MaxRefreshChance;
            }
        }
    }
}