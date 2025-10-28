using System;
using Cysharp.Threading.Tasks;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using UnityEngine.InputSystem;

namespace TeleportHome
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                // SceneLoader.Instance.LoadBaseScene(
                //         GameplayDataSettings.SceneManagement.FailLoadingScreenScene, true
                //     )
                //     .Forget();
                SceneLoader.Instance.LoadBaseScene(
                        GameplayDataSettings.SceneManagement.EvacuateScreenScene, true
                    )
                    .Forget();
            }
        }
    }
}