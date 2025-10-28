using HarmonyLib;

namespace MergeMyMOD
{
    public class WeakerEnemy
    {
        //敌人搜索时变小聋瞎
        [HarmonyPatch(typeof(AIMainBrain), "DoSearch")]
        public class EnemyIsBlind
        {
            [HarmonyPrefix]
            static void Prefix(AIMainBrain __instance,ref AIMainBrain.SearchTaskContext context)
            {
                if (!ModBehaviour.MyCustom.isWeakerEnemy)
                {
                    return;
                }
                context.searchAngle *= ModBehaviour.MyCustom.EnemySearchAngleMultiply;
                context.searchDistance *= ModBehaviour.MyCustom.EnemySearchDistanceMultiply;
            }
        }
    }
}