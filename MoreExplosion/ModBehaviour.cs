using System;
using System.Collections;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreExplosion
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
            harmony.UnpatchAll("MoreExplosion");
        }

        private void Start()
        {
            harmony = new Harmony("MoreExplosion");
            harmony.PatchAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                try
                {
                    StartCoroutine(CreateExplosion(this.GetMousePosition()));
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public IEnumerator CreateExplosion(Vector3 pos)
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.1f);
                Vector3 randomPos = pos + new Vector3(
                    UnityEngine.Random.Range(-2, 2), 0, UnityEngine.Random.Range(-2, 2)
                );
                DamageInfo dmgInfo = new DamageInfo(LevelManager.Instance.MainCharacter);
                dmgInfo.damageValue = 50;
                dmgInfo.fromWeaponItemID = 67;
                dmgInfo.armorPiercing = 50;
                LevelManager.Instance.ExplosionManager.CreateExplosion(
                    randomPos, 5, dmgInfo, ExplosionFxTypes.normal, 1f, false
                );
            }


            yield break;
        }

        public Vector3 GetMousePosition()
        {
            Vector2 v = Mouse.current.position.ReadValue();
            Ray ray = LevelManager.Instance.GameCamera.renderCamera.ScreenPointToRay(v);
            LayerMask mask = GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask;
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, mask, QueryTriggerInteraction.Ignore))
            {
                return raycastHit.point;
            }

            return LevelManager.Instance.MainCharacter.transform.position;
        }
    }
}