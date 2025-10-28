using System.Reflection;
using Duckov.UI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace WeakerEnemy
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static float blindMultiply;
  

        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("SuperPet").PatchAll();
            ModBehaviour.blindMultiply = LoadData.LoadDataFromFile(0.5f);
        }

        //敌人搜索时变小聋瞎
        [HarmonyPatch(typeof(AIMainBrain), "DoSearch")]
        public class EnemyIsBlind
        {
            [HarmonyPrefix]
            static void Prefix(AIMainBrain __instance,ref AIMainBrain.SearchTaskContext context)
            {
                context.searchAngle *= ModBehaviour.blindMultiply;
                context.searchDistance *= ModBehaviour.blindMultiply;
            }
        }

        
    }
}