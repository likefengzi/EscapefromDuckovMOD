using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Duckov.Utilities;
using ECM2;
using ECM2.Walkthrough.Ex43;
using HarmonyLib;
using UnityEngine;

namespace BetterJump
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public Harmony harmony;

        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void OnDisable()
        {
            harmony.UnpatchAll("BetterJump");
        }

        private void Start()
        {
            harmony = new Harmony("BetterJump");
            harmony.PatchAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                try
                {
                    StartCoroutine(DoJump());
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public IEnumerator DoJump()
        {
            if (LevelManager.Instance.MainCharacter.movementControl.IsOnGround)
            {
                Vector3 vector = LevelManager.Instance.MainCharacter.movementControl.Velocity;
                for (int i = 0; i < 50; i++)
                {
                    Vector3 positon = LevelManager.Instance.MainCharacter.transform.position +
                                      new Vector3(vector.x / 100f, 10f/100f, vector.z / 100f);
                    LevelManager.Instance.MainCharacter.SetPosition(positon);
                    yield return new WaitForSeconds(0.01f);
                    // Vector3 vector = 
                    //     new Vector3(0,10,0);
                    // CharacterMovement characterMovement =
                    //     LevelManager.Instance.MainCharacter.movementControl.GetComponent<CharacterMovement>();
                    // characterMovement.velocity = vector;
                    //LevelManager.Instance.MainCharacter.movementControl.SetForceMoveVelocity(vector);
                }
            }

            yield break;
        }
    }
}