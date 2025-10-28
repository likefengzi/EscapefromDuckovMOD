using HarmonyLib;
using ItemStatsSystem;

namespace MaxStack
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("MaxStack").PatchAll();
        }

        //最大堆叠数量
        [HarmonyPatch(typeof(Item), "get_Stackable")]
        public class MoreMaxStack_Stackable
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance, ref int ___maxStackCount)
            {
                if (___maxStackCount > 1 && ___maxStackCount < 99999)
                {
                    ___maxStackCount = 99999;
                    //__instance.SetInt("Count", 999);
                }
            }
        }

        [HarmonyPatch(typeof(Item), "get_MaxStackCount")]
        public class MoreMaxStack_MaxStackCount
        {
            [HarmonyPrefix]
            static void Prefix(Item __instance, ref int ___maxStackCount)
            {
                if (___maxStackCount > 1 && ___maxStackCount < 99999)
                {
                    ___maxStackCount = 99999;
                    //__instance.SetInt("Count", 999);
                }
            }
        }
    }
}