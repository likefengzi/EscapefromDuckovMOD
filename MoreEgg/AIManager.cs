using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cinemachine;
using DG.Tweening;
using Duckov;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.Utilities;
using Eflatun.SceneReference;
using HarmonyLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using NodeCanvas.Tasks.Actions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MoreEgg
{
    public class AIManager
    {
        public class EquipmentLevelFlag : MonoBehaviour
        {
            public bool flag = false;
        }

        public class ItemFlag : MonoBehaviour
        {
            public bool flag = false;
        }

        public class AIName:MonoBehaviour
        {
            public string customName;
        }
        public class AIData : MonoBehaviour
        {
            public int bossID;
            public int equipmentLevel;
            public int weaponLevel;
            public bool isBoss;
            public bool isCheat;
        }

        public struct AIDataStruct
        {
            public int bossID;
            public int equipmentLevel;
            public int weaponLevel;
            public bool isBoss;
            public bool isCheat;

            public AIDataStruct(int bossID, int equipmentLevel, int weaponLevel, bool isBoss, bool isCheat)
            {
                this.bossID = bossID;
                this.equipmentLevel = equipmentLevel;
                this.weaponLevel = weaponLevel;
                this.isBoss = isBoss;
                this.isCheat = isCheat;
            }
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
                    if (ai.leader.IsMainCharacter)
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
                ai.CharacterMainControl.PopText("遵命，老板");
            }
        }

        public static void AIRandomGotoHere(Vector3 position, float waitTime)
        {
            ModBehaviour.timer = waitTime;
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
                    {
                        list.Add(ai);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            AICharacterController aiRandom = list[UnityEngine.Random.Range(0, list.Count)];
            aiRandom.StopMove();
            aiRandom.MoveToPos(position +
                               new Vector3(UnityEngine.Random.Range(-1, 1),
                                   0, UnityEngine.Random.Range(-1, 1)));
            aiRandom.CharacterMainControl.PopText("遵命，老板");
        }

        public static Item CreateItemByID(int id)
        {
            Item item = ItemAssetsCollection.InstantiateSync(id);
            return item;
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

        public static void CheckAndUnplugSlots(AICharacterController ai, string slotTag)
        {
            Slot slot = ai.CharacterMainControl.CharacterItem.Slots[slotTag];
            try
            {
                slot.Unplug();
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        public static void CheckAndChangeGunSlots(AICharacterController ai, int weaponLevel, string slotTag,
            int itemID)
        {
            Item gun = AIManager.CreateItemByID(itemID);
            if (weaponLevel == 3)
            {
                //战术
                AIManager.PlugGunSlot(gun, "Tec", 576);
            }

            if (weaponLevel == 4)
            {
                //战术
                AIManager.PlugGunSlot(gun, "Tec", 576);
                //弹夹
                AIManager.PlugGunSlot(gun, "Mag", 548);
            }

            if (weaponLevel >= 5)
            {
                //瞄具
                AIManager.PlugGunSlot(gun, "Scope", 570);
                //枪口
                AIManager.PlugGunSlot(gun, "Muzzle", 482);
                //握把
                AIManager.PlugGunSlot(gun, "Grip", 453);
                //枪托
                AIManager.PlugGunSlot(gun, "Stock", 508);
                //战术
                AIManager.PlugGunSlot(gun, "Tec", 576);
                //弹夹
                AIManager.PlugGunSlot(gun, "Mag", 548);
            }

            ai.CharacterMainControl.CharacterItem.Slots[slotTag].Plug(gun, out Item unpluggedItem0);
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

        public static IEnumerator SaveAllAIData(int tempBossID, int equipmentLevel, int weaponLevel
            , bool isBoss, bool isCheat)
        {
            yield return new WaitForSeconds(0.3f);
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
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
                AIManager.SaveAIData(ai, tempBossID, equipmentLevel, weaponLevel, isBoss, isCheat);
            }

            yield break;
        }

        public static void SaveAIData(AICharacterController ai, int tempBossID, int equipmentLevel, int weaponLevel,
            bool isBoss, bool isCheat)
        {
            AIData data = ai.AddComponent<AIData>();
            data.bossID = tempBossID;
            data.equipmentLevel = equipmentLevel;
            data.weaponLevel = weaponLevel;
            data.isBoss = isBoss;
            data.isCheat = isCheat;
        }


        public static IEnumerator ChangeAllEquipment(int equipmentLevel, int weaponLevel, bool isBoss, bool isCustom)
        {
            yield return new WaitForSeconds(0.3f);
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
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
                if (isCustom)
                {
                    AIManager.CustomEquipment(ai);
                }
                else
                {
                    AIManager.ChangeEquipment(ai, equipmentLevel, weaponLevel, isBoss);
                }
            }

            yield break;
        }

        private static void ChangeEquipment(AICharacterController ai, int equipmentLevel, int weaponLevel, bool isBoss)
        {
            if (ai.GetComponent<EquipmentLevelFlag>() == null)
            {
                try
                {
                    ai.CharacterMainControl.CharacterItem.Inventory.SetCapacity(512);
                    if (isBoss)
                    {
                        //大急救箱
                        ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                        ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                        //ai.CharacterMainControl.PopText("正在使用制式装备券");
                        ai.CharacterMainControl.SwitchToFirstAvailableWeapon();
                        ai.AddComponent<EquipmentLevelFlag>().flag = true;
                        ai.CharacterMainControl.AddComponent<AIName>().customName = ModBehaviour.aiName;
                        return;
                    }

                    if (equipmentLevel == 0)
                    {
                        ai.CharacterMainControl.DestroyAllItem();
                        // AIManager.CheckAndUnplugSlots(ai, "Helmat");
                        // AIManager.CheckAndUnplugSlots(ai, "Armor");
                        // AIManager.CheckAndUnplugSlots(ai, "Backpack");
                        // AIManager.CheckAndUnplugSlots(ai, "FaceMask");
                    }

                    if (equipmentLevel >= 1)
                    {
                        ai.CharacterMainControl.DestroyAllItem();
                        AIManager.CheckAndChangeSlots(ai, "Backpack", ModBehaviour.backpackDict[equipmentLevel]);
                        AIManager.CheckAndChangeGunSlots(ai, weaponLevel, "PrimaryWeapon",
                            ModBehaviour.rifleGunDict[weaponLevel]);
                        for (int i = 0; i < 5; i++)
                        {
                            //子弹
                            ai.CharacterMainControl.PickupItem(
                                AIManager.CreateItemByID(ModBehaviour.bulletDict[weaponLevel])
                            );
                        }

                        AIManager.CheckAndChangeSlots(ai, "Helmat", ModBehaviour.helmatDict[equipmentLevel]);
                        AIManager.CheckAndChangeSlots(ai, "Armor", ModBehaviour.armorDict[equipmentLevel]);

                        if (equipmentLevel >= 4)
                        {
                            AIManager.CheckAndChangeSlots(ai, "FaceMask", 26);
                        }

                        if (equipmentLevel <= 2)
                        {
                            //小急救箱
                            ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(17));
                        }

                        if (equipmentLevel >= 3)
                        {
                            for (int i = 0; i < equipmentLevel / 2; i++)
                            {
                                //大急救箱
                                ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(15));
                            }

                            ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(941));
                            ai.CharacterMainControl.PickupItem(AIManager.CreateItemByID(942));
                        }

                        ai.CharacterMainControl.PopText("正在使用制式装备券");
                    }

                    ai.CharacterMainControl.SwitchToFirstAvailableWeapon();
                    ai.AddComponent<EquipmentLevelFlag>().flag = true;
                    ai.CharacterMainControl.AddComponent<AIName>().customName = ModBehaviour.aiName;
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        private static void CustomEquipment(AICharacterController ai)
        {
            if (ai.GetComponent<EquipmentLevelFlag>() == null)
            {
                try
                {
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
                    ai.AddComponent<EquipmentLevelFlag>().flag = true;
                    ai.CharacterMainControl.AddComponent<AIName>().customName = ModBehaviour.aiName;
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
                    if (ai.leader.IsMainCharacter)
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
                AIManager.KillAI(ai);
            }
        }

        public static void KillAI(AICharacterController ai)
        {
            List<Item> list = new List<Item>();
            foreach (Item item in ai.CharacterMainControl.CharacterItem.Inventory)
            {
                list.Add(item);
            }

            foreach (Slot slot in ai.CharacterMainControl.CharacterItem.Slots)
            {
                if (slot.Content)
                {
                    list.Add(slot.Content);
                }
            }

            Inventory inventory = AIManager.CreateLootbox(ai).Inventory;
            foreach (Item item in list)
            {
                try
                {
                    UnityEngine.Object.DestroyImmediate(item.GetComponent<ItemFlag>());
                }
                catch (Exception e)
                {
                    var a = e;
                }

                inventory.AddAndMerge(item);
            }

            inventory.Sort();
            ai.CharacterMainControl.DestroyCharacter();
        }

        public static void SearchAndPickupAndHeal()
        {
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
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
                if (AIManager.HealSelf(ai))
                {
                }
                else
                {
                    AIManager.SearchAndPickup(ai);
                    ai.CharacterMainControl.SwitchToFirstAvailableWeapon();
                    ai.CharacterMainControl.PopText("正在舔包");
                }
            }
        }

        private static bool HealSelf(AICharacterController ai)
        {
            if (ai.CharacterMainControl.Health.CurrentHealth /
                ai.CharacterMainControl.Health.MaxHealth <= 0.9f)
            {
                foreach (Item item in ai.CharacterMainControl.CharacterItem.Inventory)
                {
                    if (item.TypeID == 10 || item.TypeID == 20 ||
                        item.TypeID == 15 || item.TypeID == 16 || item.TypeID == 17)
                    {
                        ai.CharacterMainControl.UseItem(item);
                        ai.CharacterMainControl.PopText("正在打药");
                        return true;
                    }
                }
            }

            return false;
        }

        public static void SearchAndPickup(AICharacterController ai)
        {
            Collider[] cols = new Collider[99999];

            int num = Physics.OverlapSphereNonAlloc(
                ai.CharacterMainControl.transform.position,
                5f,
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
                        if (lootbox && lootbox.Inventory && lootbox.Inventory.Content != null &&
                            lootbox.Inventory.Content.Count > 0)
                        {
                            List<Item> list = new List<Item>();
                            foreach (Item item in lootbox.Inventory.Content)
                            {
                                if (item.GetTotalRawValue() >= ModBehaviour.aiPickupMoney)
                                {
                                    list.Add(item);
                                }
                            }

                            for (int j = list.Count - 1; j >= 0; j--)
                            {
                                Item item = list[j];
                                try
                                {
                                    if (ai.CharacterMainControl.CharacterItem.Inventory.GetFirstEmptyPosition() < 0)
                                    {
                                        //return;
                                    }

                                    if (ai.CharacterMainControl.PickupItem(item))
                                    {
                                        item.AddComponent<ItemFlag>().flag = true;
                                    }
                                }
                                catch (Exception e)
                                {
                                    var a = e;
                                }
                            }
                        }
                    }
                    else
                    {
                        InteractablePickup pickup = collider.GetComponent<InteractablePickup>();
                        if (pickup && pickup.ItemAgent && pickup.ItemAgent.Item)
                        {
                            try
                            {
                                Item item = pickup.ItemAgent.Item;
                                if (ai.CharacterMainControl.CharacterItem.Inventory.GetFirstEmptyPosition() < 0)
                                {
                                    //return;
                                }
                                else
                                {
                                    if (item.GetTotalRawValue() >= ModBehaviour.aiPickupMoney)
                                    {
                                        if (ai.CharacterMainControl.PickupItem(item))
                                        {
                                            item.AddComponent<ItemFlag>().flag = true;
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
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public static void DropItem()
        {
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
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
                AIManager.DropItem(ai);
            }
        }

        public static void DropItem(AICharacterController ai)
        {
            List<Item> list = new List<Item>();
            foreach (Item item in ai.CharacterMainControl.CharacterItem.Inventory)
            {
                if (item.GetComponent<ItemFlag>())
                {
                    list.Add(item);
                }
            }

            if (list.Count == 0)
            {
                return;
            }

            Inventory inventory = AIManager.CreateLootbox(ai).Inventory;
            foreach (Item item in list)
            {
                UnityEngine.Object.DestroyImmediate(item.GetComponent<ItemFlag>());

                //item.Drop(ai.CharacterMainControl.transform.position, true, Vector3.forward, 360);
                inventory.AddAndMerge(item);
            }

            inventory.Sort();

            ai.CharacterMainControl.PopText("正在整理背包");
        }

        public static InteractableLootbox CreateLootbox(AICharacterController ai)
        {
            InteractableLootbox interactableLootbox =
                UnityEngine.Object.Instantiate<InteractableLootbox>(
                    ai.CharacterMainControl.deadLootBoxPrefab,
                    ai.CharacterMainControl.transform.position,
                    ai.CharacterMainControl.transform.rotation
                );

            MultiSceneCore.MoveToActiveWithScene(interactableLootbox.gameObject,
                SceneManager.GetActiveScene().buildIndex);
            interactableLootbox.AddComponent<Inventory>();
            Inventory inventory = interactableLootbox.Inventory;
            inventory.SetCapacity(512);
            return interactableLootbox;
        }

        public static void AllReload()
        {
            AICharacterController[] aiCharacterControllers = Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
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
                AIManager.ReloadForce(ai);
            }
        }

        private static void ReloadForce(AICharacterController ai)
        {
            if (ai.CharacterMainControl.TryToReload())
            {
                ai.CharacterMainControl.PopText("正在换弹");
            }
        }

        public static void SaveAllAIDataAndDestroy()
        {
            ModBehaviour.aiDataList = new List<AIDataStruct>();
            AICharacterController[] aiCharacterControllers =
                Resources.FindObjectsOfTypeAll<AICharacterController>();
            List<AICharacterController> list = new List<AICharacterController>();
            foreach (AICharacterController ai in aiCharacterControllers)
            {
                try
                {
                    if (ai.leader.IsMainCharacter)
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
                AIData data = ai.GetComponent<AIData>();
                if (data != null)
                {
                    if (!data.isCheat)
                    {
                        ModBehaviour.aiDataList.Add(new AIDataStruct(
                            data.bossID,
                            data.equipmentLevel,
                            data.weaponLevel,
                            data.isBoss,
                            data.isCheat
                        ));
                    }
                    
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].CharacterMainControl.DestroyCharacter();
            }
        }

        public static void LoadAllAIData()
        {
            try
            {
                if (LevelManager.Instance.IsBaseLevel)
                {
                    return;
                }

                foreach (AIDataStruct data in ModBehaviour.aiDataList)
                {
                    Debug.Log("bossID" + data.bossID);

                    ModBehaviour.Spawn(
                        data.bossID,
                        5f,
                        data.equipmentLevel,
                        data.weaponLevel,
                        false,
                        data.isBoss,
                        data.isCheat
                    );
                }

                ModBehaviour.aiDataList = new List<AIDataStruct>();
            }
            catch (Exception e)
            {
                var a = e;
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

        [HarmonyPatch(typeof(HealthBar), "LateUpdate")]
        public class ChangeName
        {
            [HarmonyPrefix]
            static void Prefix(HealthBar __instance, TextMeshProUGUI ___nameText)
            {
                try
                {
                    if (__instance.target.team == LevelManager.Instance.MainCharacter.Team)
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
                                name="护航";
                            }
                            ___nameText.text = name;
                        }
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        [HarmonyPatch(typeof(SceneLoader), "LoadScene",
            new Type[]
            {
                typeof(SceneReference), // sceneReference
                typeof(SceneReference), // overrideCurtainScene (可选)
                typeof(bool), // clickToContinue
                typeof(bool), // notifyEvacuation
                typeof(bool), // doCircleFade
                typeof(bool), // useLocation
                typeof(MultiSceneLocation), // location
                typeof(bool), // saveToFile
                typeof(bool) // hideTips
            }
        )]
        public class AINoDisappear
        {
            [HarmonyPrefix]
            static void Prefix(SceneLoader __instance, SceneReference sceneReference)
            {
                AIManager.SaveAllAIDataAndDestroy();
                if (sceneReference == GameplayDataSettings.SceneManagement.MainMenuScene)
                {
                    ModBehaviour.aiDataList = new List<AIDataStruct>();
                }

                if (sceneReference == GameplayDataSettings.SceneManagement.BaseScene)
                {
                    ModBehaviour.aiDataList = new List<AIDataStruct>();
                }

                if (sceneReference == null)
                {
                    ModBehaviour.aiDataList = new List<AIDataStruct>();
                }
            }
        }

        [HarmonyPatch(typeof(CinemachineBrain), "OnSceneLoaded")]
        public class LoadAIDataAndSpawn
        {
            [HarmonyPrefix]
            static void Prefix(CinemachineBrain __instance)
            {
                AIManager.LoadAllAIData();
            }
        }

        [HarmonyPatch(typeof(MultiSceneCore), "LoadSubScene")]
        public class LoadAIDataAndSpawnMultiScene
        {
            [HarmonyPrefix]
            static void Prefix(MultiSceneCore __instance)
            {
                AIManager.SaveAllAIDataAndDestroy();
            }

            [HarmonyPostfix]
            static void Postfix(MultiSceneCore __instance)
            {
                AIManager.LoadAllAIData();
            }
        }

        [HarmonyPatch(typeof(SimpleTeleporter), "Teleport")]
        public class TeleportAllAI
        {
            [HarmonyPostfix]
            static void Postfix(SimpleTeleporter __instance, CharacterMainControl targetCharacter)
            {
                if (targetCharacter.IsMainCharacter)
                {
                    ModBehaviour.aiDataList = new List<AIDataStruct>();
                    AICharacterController[] aiCharacterControllers =
                        Resources.FindObjectsOfTypeAll<AICharacterController>();
                    List<AICharacterController> list = new List<AICharacterController>();
                    foreach (AICharacterController ai in aiCharacterControllers)
                    {
                        try
                        {
                            if (ai.leader.IsMainCharacter)
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
                        ai.CharacterMainControl.SetPosition(__instance.target.position);
                    }
                }
            }
        }
    }
}