using System;
using System.Reflection;
using Duckov.UI;
using Duckov.Utilities;
using HarmonyLib;
using ParadoxNotion;
using UnityEngine;
using UnityEngine.UI;

namespace Teaming
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void Start()
        {
            new Harmony("Teaming").PatchAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                try
                {
                    this.ChangeTeam();
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public void ChangeTeam()
        {
            Vector3 searchCenter = LevelManager.Instance.MainCharacter.transform.position;
            float searchDistance = 5f;
            Collider[] cols = new Collider[10000];
            LayerMask dmgReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;

            Teams characterTeam = LevelManager.Instance.MainCharacter.Team;
            CharacterMainControl fromCharacter = LevelManager.Instance.MainCharacter;

            int num = Physics.OverlapSphereNonAlloc(searchCenter, searchDistance, cols,
                dmgReceiverLayers,
                QueryTriggerInteraction.Collide);
            if (num > 0)
            {
                foreach (Collider collider in cols)
                {
                    if (collider.gameObject.IsInLayerMask(dmgReceiverLayers))
                    {
                        DamageReceiver dmgReceiverTemp = collider.GetComponent<DamageReceiver>();
                        if (dmgReceiverTemp != null && dmgReceiverTemp.health)
                        {
                            CharacterMainControl characterMainControl = dmgReceiverTemp.health.TryGetCharacter();
                            if (characterMainControl)
                            {
                            }

                            if (dmgReceiverTemp && dmgReceiverTemp.health != null)
                            {
                                if (Team.IsEnemy(characterTeam, dmgReceiverTemp.Team))
                                {
                                    characterMainControl.SetTeam(characterTeam);
                                    AICharacterController componentInChildren = characterMainControl.GetComponentInChildren<AICharacterController>();
                                    if (componentInChildren)
                                    {
                                        PetAI component = componentInChildren.GetComponent<PetAI>();
                                        if (component)
                                        {
                                            component.SetMaster(fromCharacter);
                                        }
                                        componentInChildren.leader = fromCharacter;
                                        if (fromCharacter)
                                        {
                                            componentInChildren.CharacterMainControl.SetTeam(fromCharacter.Team);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}