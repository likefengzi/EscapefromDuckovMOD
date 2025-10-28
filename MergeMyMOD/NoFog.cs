using HarmonyLib;

namespace MergeMyMOD
{
    public class NoFog
    {
        //去除迷雾
        [HarmonyPatch(typeof(FogOfWarManager), "Update")]
        public class NoFog_Update
        {
            [HarmonyPrefix]
            static void Prefix(FogOfWarManager __instance, ref bool ___allVision)
            {
                ___allVision = ModBehaviour.MyCustom.isNoFog;
            }
        }
    }
}