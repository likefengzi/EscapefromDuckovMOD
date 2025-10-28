using System;
using System.Collections;
using System.Collections.Generic;
using Duckov.UI;
using HarmonyLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using NodeCanvas.Tasks.Actions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace EVE
{
    public class AIManager
    {
        public class EquipmentFlag : MonoBehaviour
        {
            public bool flag = false;
        }

        public class AIName : MonoBehaviour
        {
            public string customName;
        }

        public class AITeam : MonoBehaviour
        {
            public Teams team;
        }

        public static void AIGotoHere(Vector3 position, float waitTime)
        {
            ModBehaviour.timer = waitTime;
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (!ai.CharacterMainControl.IsMainCharacter)
                    {
                        list.Add(ai);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            foreach (AICharacterController ai in list)
            {
                ai.StopMove();
                ai.MoveToPos(position +
                             new Vector3(UnityEngine.Random.Range(-1, 1),
                                 0, UnityEngine.Random.Range(-1, 1)));
                ai.CharacterMainControl.PopText("正在前进");
            }
        }

        public static Item CreateItemByID(int id)
        {
            Item item = ItemAssetsCollection.InstantiateSync(id);
            return item;
        }

        public static IEnumerator ChangeAllEquipment(bool isBoss)
        {
            yield return new WaitForSeconds(0.3f);
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (!ai.CharacterMainControl.IsMainCharacter)
                    {
                        list.Add(ai);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            foreach (AICharacterController ai in list)
            {
                AIManager.CustomEquipment(ai, isBoss);
            }

            yield break;
        }

        private static void CustomEquipment(AICharacterController ai, bool isBoss)
        {
            if (ai.GetComponent<EquipmentFlag>() == null)
            {
                try
                {
                    if (isBoss)
                    {
                        //大急救箱
                        ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                        ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                        //ai.CharacterMainControl.PopText("正在使用制式装备券");
                        ai.CharacterMainControl.SwitchToFirstAvailableWeapon();
                        ai.AddComponent<EquipmentFlag>().flag = true;
                        ai.CharacterMainControl.AddComponent<AIName>().customName = ModBehaviour.aiName;
                        return;
                    }

                    ai.CharacterMainControl.DestroyAllItem();
                    ai.CharacterMainControl.CharacterItem.Inventory.SetCapacity(512);
                    Item gun = AIManager.CreateItemByID(ModBehaviour.primaryWeaponID);

                    //瞄具
                    AIManager.PlugGunSlot(gun, "Scope", ModBehaviour.primaryWeaponScopeSlotID);
                    //枪口
                    AIManager.PlugGunSlot(gun, "Muzzle", ModBehaviour.primaryWeaponMuzzleSlotID);
                    //握把
                    AIManager.PlugGunSlot(gun, "Grip", ModBehaviour.primaryWeaponGripSlotID);
                    //枪托
                    AIManager.PlugGunSlot(gun, "Stock", ModBehaviour.primaryWeaponStockSlotID);
                    //战术
                    AIManager.PlugGunSlot(gun, "Tec", ModBehaviour.primaryWeaponTecSlotID);
                    //弹夹
                    AIManager.PlugGunSlot(gun, "Mag", ModBehaviour.primaryWeaponMagSlotID);
                    ai.CharacterMainControl.CharacterItem.Slots["PrimaryWeapon"].Plug(gun, out Item unpluggedItem0);
                    for (int i = 0; i < 10; i++)
                    {
                        //子弹
                        ai.CharacterMainControl.PickupItem(
                            AIManager.CreateItemByID(ModBehaviour.bulletID)
                        );
                    }

                    AIManager.CheckAndChangeSlots(ai, "MeleeWeapon", ModBehaviour.meleeWeaponID);
                    AIManager.CheckAndChangeSlots(ai, "Helmat", ModBehaviour.helmatID);
                    AIManager.CheckAndChangeSlots(ai, "Armor", ModBehaviour.armorID);
                    AIManager.CheckAndChangeSlots(ai, "FaceMask", ModBehaviour.faceMaskID);
                    AIManager.CheckAndChangeSlots(ai, "Headset", ModBehaviour.headsetID);
                    AIManager.CheckAndChangeSlots(ai, "Backpack", ModBehaviour.backpackID);
                    AIManager.CheckAndChangeSlots(ai, "Totem1", ModBehaviour.totem1ID);
                    AIManager.CheckAndChangeSlots(ai, "Totem2", ModBehaviour.totem2ID);
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(941));
                    ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(942));

                    ai.CharacterMainControl.PopText("正在使用制式装备券");
                    ai.CharacterMainControl.SwitchToFirstAvailableWeapon();
                    ai.AddComponent<EquipmentFlag>().flag = true;
                    ai.CharacterMainControl.AddComponent<AIName>().customName = ModBehaviour.aiName;
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public static void PlugGunSlot(Item gun, string slotTag, int itemID)
        {
            try
            {
                gun.Slots[slotTag].Plug(AIManager.CreateItemByID(itemID), out Item unpluggedItem);
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        public static void CheckAndChangeSlots(AICharacterController ai, string slotTag, int itemID)
        {
            Slot slot = ai.CharacterMainControl.CharacterItem.Slots[slotTag];
            try
            {
                slot.Plug(AIManager.CreateItemByID(itemID), out Item unpluggedItem);
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        public static IEnumerator MarkAllTeamAndLeader()
        {
            yield return new WaitForSeconds(0.3f);
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (!ai.CharacterMainControl.IsMainCharacter)
                    {
                        list.Add(ai);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            foreach (AICharacterController ai in list)
            {
                AIManager.MarkAITeamAndLeader(ai, ModBehaviour.aiTeamDict[ModBehaviour.aiTeam]);
            }

            yield break;
        }

        public static void ChangeAllTeam()
        {
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (!ai.CharacterMainControl.IsMainCharacter)
                    {
                        list.Add(ai);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            foreach (AICharacterController ai in list)
            {
                AIManager.ChangeAITeam(ai);
            }
        }

        public static void MarkAITeamAndLeader(AICharacterController ai, Teams team)
        {
            if (ai.GetComponent<AITeam>() == null)
            {
                try
                {
                    //ai.CharacterMainControl.SetTeam(team);
                    PetAI component = ai.GetComponent<PetAI>();
                    if (component)
                    {
                        component.master = null;
                    }

                    ai.leader = null;
                    ai.AddComponent<AITeam>().team = team;
                    if (ai.GetComponent<AIName>() == null)
                    {
                        ai.CharacterMainControl.AddComponent<AIName>().customName = ModBehaviour.aiName;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public static void ChangeAITeam(AICharacterController ai)
        {
            if (ai.GetComponent<AITeam>() != null)
            {
                try
                {
                    ai.CharacterMainControl.SetTeam(ai.GetComponent<AITeam>().team);
                    ai.CharacterMainControl.PopText("即将开战");
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public static void KillAllAI()
        {
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (!ai.CharacterMainControl.IsMainCharacter)
                    {
                        list.Add(ai);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            for (int i = list.Count - 1; i >= 0; i--)
            {
                list[i].CharacterMainControl.DestroyCharacter();
            }
            Collider[] cols = new Collider[99999];

            int num = Physics.OverlapSphereNonAlloc(
                LevelManager.Instance.MainCharacter.transform.position,
                100f,
                cols
            );
            for (int i = 0; i < num; i++)
            {
                try
                {
                    Collider collider = cols[i];

                    if (collider.GetComponent<InteractableLootbox>())
                    {
                        InteractableLootbox lootbox = collider.GetComponent<InteractableLootbox>();
                        lootbox.gameObject.SetActive(false);
                    }
                    else
                    {
                        InteractablePickup pickup = collider.GetComponent<InteractablePickup>();
                        if (pickup && pickup.ItemAgent && pickup.ItemAgent.Item)
                        {
                            try
                            {
                                Item item = pickup.ItemAgent.Item;
                                item.DestroyTree();
                            }
                            catch (Exception e)
                            {
                                var a = e;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public static void MoveAllAI(Vector3 position,float waitTime)
        {
            AIManager.AIGotoHere(position, waitTime);
        }

        [HarmonyPatch(typeof(HealthBar), "LateUpdate")]
        public class ChangeName
        {
            [HarmonyPrefix]
            static void Prefix(HealthBar __instance, TextMeshProUGUI ___nameText)
            {
                try
                {
                    if (true)
                    {
                        //___nameText.gameObject.SetActive(true);
                        if (__instance.target.TryGetCharacter().IsMainCharacter)
                        {
                            //___nameText.text = "老板";
                        }
                        else
                        {
                            string name = __instance.target.TryGetCharacter().GetComponent<AIName>().customName;
                            if (string.IsNullOrEmpty(name))
                            {
                            }
                            else
                            {
                                ___nameText.text = name;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        [HarmonyPatch(typeof(TraceTarget), "OnUpdate")]
        public class NoComeback
        {
            [HarmonyPrefix]
            static bool Prefix(TraceTarget __instance)
            {
                try
                {
                    if (__instance.agent.leader.IsMainCharacter && ModBehaviour.timer >= 0)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }


                return true;
            }
        }

        [HarmonyPatch(typeof(TraceTarget), "OnExecute")]
        public class NoComeback2
        {
            [HarmonyPrefix]
            static bool Prefix(TraceTarget __instance)
            {
                try
                {
                    if (__instance.agent.leader.IsMainCharacter && ModBehaviour.timer >= 0)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(TraceTarget), "OnStop")]
        public class NoComeback3
        {
            [HarmonyPrefix]
            static bool Prefix(TraceTarget __instance)
            {
                try
                {
                    if (__instance.agent.leader.IsMainCharacter && ModBehaviour.timer >= 0)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(StopMoving), "OnExecute")]
        public class NoComeback4
        {
            [HarmonyPrefix]
            static bool Prefix(TraceTarget __instance)
            {
                try
                {
                    if (__instance.agent.leader.IsMainCharacter && ModBehaviour.timer >= 0)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Team), nameof(Team.IsEnemy))]
        public class NoWar
        {
            [HarmonyPrefix]
            static bool Prefix(Teams selfTeam, Teams targetTeam, ref bool __result)
            {
                try
                {
                    if (selfTeam==Teams.player||targetTeam==Teams.player)
                    {
                        __result = false;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return true;
            }
        }
        [HarmonyPatch(typeof(AI_PathControl), nameof(AI_PathControl.MoveToPos))]
        public class AIMove
        {
            [HarmonyPrefix]
            static bool Prefix(AI_PathControl __instance)
            {
                try
                {
                    if (ModBehaviour.isNoMove)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return true;
            }
        }
    }
}