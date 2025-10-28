using System;
using System.Collections;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace MoreEgg
{
    public class SpawnEgg
    {
        public static void Init()
        {
            Egg[] allEggs = Resources.FindObjectsOfTypeAll<Egg>();
            ModBehaviour.eggPrefab = allEggs[UnityEngine.Random.Range(0, allEggs.Length)];
            //CharacterRandomPreset[] allCharacters = Resources.FindObjectsOfTypeAll<CharacterRandomPreset>();
            List<CharacterRandomPreset> allCharacters = GameplayDataSettings.CharacterRandomPresetData.presets;
            foreach (CharacterRandomPreset character in allCharacters)
            {
                try
                {
                    if (character.nameKey == "Cname_Usec" &&
                        ModBehaviour.spawnCharacterDict[0].spawnCharacter == character)
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            foreach (CharacterRandomPreset character in allCharacters)
            {
                try
                {
                    int id = ModBehaviour.spawnCharacterNameDict[character.nameKey];
                    ModBehaviour.spawnCharacterDict[id].spawnCharacter = UnityEngine.Object.Instantiate(character);
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            //ModBehaviour.spawnCharacter = ModBehaviour.spawnCharacterDict[0].spawnCharacter;
        }

        public static IEnumerator SpawnEggFunc(int tempBossID,float waitTime,int equipmentLevel,int weaponLevel,bool isBoss,bool isCheat,bool isCustom)
        {
            yield return new WaitForSeconds(waitTime);
            CharacterRandomPreset spawnCharacter = ModBehaviour.spawnCharacterDict[tempBossID].spawnCharacter;
            //this.characterBackup = ScriptableObject.CreateInstance<CharacterRandomPreset>();
            if (equipmentLevel==0)
            {
                spawnCharacter = SpawnEgg.SetZeroUsec(spawnCharacter);
            }
            if (equipmentLevel==1)
            {
                spawnCharacter = SpawnEgg.SetNormalUsec(spawnCharacter);
            }
            if (equipmentLevel==2||equipmentLevel==3||equipmentLevel==4)
            {
                spawnCharacter = SpawnEgg.SetGoodUsec(spawnCharacter);
            }
            if (equipmentLevel==5)
            {
                spawnCharacter = SpawnEgg.SetWonderfulUsec(spawnCharacter);
            }
            if (equipmentLevel==6)
            {
                spawnCharacter = SpawnEgg.SetSuperUsec(spawnCharacter);
            }

            if (isCheat)
            {
                spawnCharacter = SpawnEgg.SetUsec(spawnCharacter);
            }

            Vector3 vector3 = new Vector3(UnityEngine.Random.Range(-1,1),2,UnityEngine.Random.Range(-1,1));
            Egg egg = UnityEngine.Object.Instantiate<Egg>(ModBehaviour.eggPrefab,
                LevelManager.Instance.MainCharacter.transform.position+vector3, Quaternion.identity);

            egg.Init(LevelManager.Instance.MainCharacter.transform.position+vector3,
                LevelManager.Instance.MainCharacter.CurrentAimDirection * 1f,
                LevelManager.Instance.MainCharacter, spawnCharacter, 0.001f);

            //StartCoroutine(this.LoadUsecAsync(this.spawnCharacter, this.characterBackup));

            LevelManager.Instance.MainCharacter.StartCoroutine(AIManager.ChangeAllEquipment(equipmentLevel,weaponLevel,isBoss,isCustom));
            if (isCustom)
            {
                isCheat = true;
            }
            LevelManager.Instance.MainCharacter.StartCoroutine(AIManager.SaveAllAIData(tempBossID,equipmentLevel,weaponLevel,isBoss,isCheat));
            yield break;
        }

        public static CharacterRandomPreset SetUsec(CharacterRandomPreset characterOrigin)
        {
            CharacterRandomPreset character = UnityEngine.Object.Instantiate(characterOrigin);
            //characterBackup.canDash = character.canDash;
            character.canDash = true;

            //characterBackup.forceTracePlayerDistance = character.forceTracePlayerDistance;
            //character.forceTracePlayerDistance = 0.001f;

            character.damageMultiplier *= ModBehaviour.aiDamageMultiply;

            //枪线散布
            character.gunScatterMultiplier *= ModBehaviour.aiGunScatterMultiply;

            //characterBackup.health = character.health;
            character.health = 100 * ModBehaviour.aiHealthMultiply;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;
            
            character.moveSpeedFactor *= 1.5f*ModBehaviour.aiSpeedMultiply;

            character.nightReactionTimeFactor = 0.001f;

            character.nightVisionAbility = 1f;

            //characterBackup.patrolRange = character.patrolRange;
            //character.patrolRange = 999f;

            //characterBackup.patrolTurnSpeed = character.patrolTurnSpeed;
            //character.patrolTurnSpeed = 999f;

            //characterBackup.reactionTime = character.reactionTime;
            character.reactionTime = 0.001f;

            //characterBackup.shootCanMove = character.shootCanMove;
            character.shootCanMove = true;

            character.shootDelay = 0.001f;

            //characterBackup.shootTimeRange = character.shootTimeRange;
            //character.shootTimeRange = new Vector2(999, 999);

            //characterBackup.shootTimeSpaceRange = character.shootTimeSpaceRange;
            character.shootTimeSpaceRange = new Vector2(0.001f, 0.001f);

            //characterBackup.showHealthBar = character.showHealthBar;
            character.showHealthBar = true;

            //characterBackup.showName = character.showName;
            character.showName = true;

            //characterBackup.sightAngle = character.sightAngle;
            character.sightAngle = 360f;

            character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            return character;
        }
        
        private static CharacterRandomPreset SetZeroUsec(CharacterRandomPreset characterOrigin)
        {
            CharacterRandomPreset character = UnityEngine.Object.Instantiate(characterOrigin);
            //characterBackup.canDash = character.canDash;
            character.canDash = true;

            //characterBackup.forceTracePlayerDistance = character.forceTracePlayerDistance;
            //character.forceTracePlayerDistance = 0.001f;

            //character.damageMultiplier *= ModBehaviour.aiDamageMultiply;

            //枪线散布
            character.gunScatterMultiplier *= 0.5f;

            //characterBackup.health = character.health;
            character.health = 100;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;

            character.moveSpeedFactor = 1.5f;

            character.nightReactionTimeFactor = 0.001f;

            character.nightVisionAbility = 1f;

            //characterBackup.patrolRange = character.patrolRange;
            //character.patrolRange = 999f;

            //characterBackup.patrolTurnSpeed = character.patrolTurnSpeed;
            //character.patrolTurnSpeed = 999f;

            //characterBackup.reactionTime = character.reactionTime;
            character.reactionTime = 0.001f;

            //characterBackup.shootCanMove = character.shootCanMove;
            character.shootCanMove = true;

            character.shootDelay = 0.001f;

            //characterBackup.shootTimeRange = character.shootTimeRange;
            character.shootTimeRange = new Vector2(0.001f, 0.001f);

            //characterBackup.shootTimeSpaceRange = character.shootTimeSpaceRange;
            character.shootTimeSpaceRange = new Vector2(0.001f, 0.001f);

            //characterBackup.showHealthBar = character.showHealthBar;
            character.showHealthBar = true;

            //characterBackup.showName = character.showName;
            character.showName = true;

            //characterBackup.sightAngle = character.sightAngle;
            character.sightAngle = 360f;

            //character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            character.sightDistance *= 1.5f;
            
            return character;
        }
        public static CharacterRandomPreset SetNormalUsec(CharacterRandomPreset characterOrigin)
        {
            CharacterRandomPreset character = UnityEngine.Object.Instantiate(characterOrigin);
            //characterBackup.canDash = character.canDash;
            character.canDash = true;

            //characterBackup.forceTracePlayerDistance = character.forceTracePlayerDistance;
            //character.forceTracePlayerDistance = 0.001f;

            //character.damageMultiplier *= ModBehaviour.aiDamageMultiply;

            //枪线散布
            character.gunScatterMultiplier *= 0.5f;

            //characterBackup.health = character.health;
            character.health = 100;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;
            
            character.moveSpeedFactor = 1.5f;

            //character.nightReactionTimeFactor = 0.001f;

            character.nightVisionAbility = 1f;

            //characterBackup.patrolRange = character.patrolRange;
            //character.patrolRange = 999f;

            //characterBackup.patrolTurnSpeed = character.patrolTurnSpeed;
            //character.patrolTurnSpeed = 999f;

            //characterBackup.reactionTime = character.reactionTime;
            //character.reactionTime = 0.001f;

            //characterBackup.shootCanMove = character.shootCanMove;
            character.shootCanMove = true;

            //character.shootDelay = 0.001f;

            //characterBackup.shootTimeRange = character.shootTimeRange;
            //character.shootTimeRange = new Vector2(0.001f, 0.001f);

            //characterBackup.shootTimeSpaceRange = character.shootTimeSpaceRange;
            //character.shootTimeSpaceRange = new Vector2(0.001f, 0.001f);

            //characterBackup.showHealthBar = character.showHealthBar;
            character.showHealthBar = true;

            //characterBackup.showName = character.showName;
            character.showName = true;

            //characterBackup.sightAngle = character.sightAngle;
            character.sightAngle = 180f;

            //character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            return character;
        }
        public static CharacterRandomPreset SetGoodUsec(CharacterRandomPreset characterOrigin)
        {
            CharacterRandomPreset character = UnityEngine.Object.Instantiate(characterOrigin);
            //characterBackup.canDash = character.canDash;
            character.canDash = true;

            //characterBackup.forceTracePlayerDistance = character.forceTracePlayerDistance;
            //character.forceTracePlayerDistance = 0.001f;

            //character.damageMultiplier *= ModBehaviour.aiDamageMultiply;

            //枪线散布
            character.gunScatterMultiplier *= 0.25f;

            //characterBackup.health = character.health;
            character.health = 100;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;
            
            character.moveSpeedFactor = 1.5f;

            character.nightReactionTimeFactor = 0.001f;

            character.nightVisionAbility = 1f;

            //characterBackup.patrolRange = character.patrolRange;
            //character.patrolRange = 999f;

            //characterBackup.patrolTurnSpeed = character.patrolTurnSpeed;
            //character.patrolTurnSpeed = 999f;

            //characterBackup.reactionTime = character.reactionTime;
            character.reactionTime = 0.001f;

            //characterBackup.shootCanMove = character.shootCanMove;
            character.shootCanMove = true;

            character.shootDelay = 0.001f;

            //characterBackup.shootTimeRange = character.shootTimeRange;
            character.shootTimeRange = new Vector2(0.001f, 0.001f);

            //characterBackup.shootTimeSpaceRange = character.shootTimeSpaceRange;
            character.shootTimeSpaceRange = new Vector2(0.001f, 0.001f);

            //characterBackup.showHealthBar = character.showHealthBar;
            character.showHealthBar = true;

            //characterBackup.showName = character.showName;
            character.showName = true;

            //characterBackup.sightAngle = character.sightAngle;
            character.sightAngle = 360f;

            //character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            return character;
        }
        public static CharacterRandomPreset SetWonderfulUsec(CharacterRandomPreset characterOrigin)
        {
            CharacterRandomPreset character = UnityEngine.Object.Instantiate(characterOrigin);
            //characterBackup.canDash = character.canDash;
            character.canDash = true;

            //characterBackup.forceTracePlayerDistance = character.forceTracePlayerDistance;
            //character.forceTracePlayerDistance = 0.001f;

            //character.damageMultiplier *= ModBehaviour.aiDamageMultiply;

            //枪线散布
            character.gunScatterMultiplier *= 0.001f;

            //characterBackup.health = character.health;
            character.health = 150;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;
            
            character.moveSpeedFactor = 1.5f;

            character.nightReactionTimeFactor = 0.001f;

            character.nightVisionAbility = 1f;

            //characterBackup.patrolRange = character.patrolRange;
            //character.patrolRange = 999f;

            //characterBackup.patrolTurnSpeed = character.patrolTurnSpeed;
            //character.patrolTurnSpeed = 999f;

            //characterBackup.reactionTime = character.reactionTime;
            character.reactionTime = 0.001f;

            //characterBackup.shootCanMove = character.shootCanMove;
            character.shootCanMove = true;

            character.shootDelay = 0.001f;

            //characterBackup.shootTimeRange = character.shootTimeRange;
            character.shootTimeRange = new Vector2(0.001f, 0.001f);

            //characterBackup.shootTimeSpaceRange = character.shootTimeSpaceRange;
            character.shootTimeSpaceRange = new Vector2(0.001f, 0.001f);

            //characterBackup.showHealthBar = character.showHealthBar;
            character.showHealthBar = true;

            //characterBackup.showName = character.showName;
            character.showName = true;

            //characterBackup.sightAngle = character.sightAngle;
            character.sightAngle = 360f;

            //character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            return character;
        }
        public static CharacterRandomPreset SetSuperUsec(CharacterRandomPreset characterOrigin)
        {
            CharacterRandomPreset character = UnityEngine.Object.Instantiate(characterOrigin);
            //characterBackup.canDash = character.canDash;
            character.canDash = true;

            //characterBackup.forceTracePlayerDistance = character.forceTracePlayerDistance;
            //character.forceTracePlayerDistance = 0.001f;

            //character.damageMultiplier *= ModBehaviour.aiDamageMultiply;

            //枪线散布
            character.gunScatterMultiplier *= 0.001f;

            //characterBackup.health = character.health;
            character.health = 200;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;

            character.moveSpeedFactor = 1.5f;

            character.nightReactionTimeFactor = 0.001f;

            character.nightVisionAbility = 1f;

            //characterBackup.patrolRange = character.patrolRange;
            //character.patrolRange = 999f;

            //characterBackup.patrolTurnSpeed = character.patrolTurnSpeed;
            //character.patrolTurnSpeed = 999f;

            //characterBackup.reactionTime = character.reactionTime;
            character.reactionTime = 0.001f;

            //characterBackup.shootCanMove = character.shootCanMove;
            character.shootCanMove = true;

            character.shootDelay = 0.001f;

            //characterBackup.shootTimeRange = character.shootTimeRange;
            character.shootTimeRange = new Vector2(0.001f, 0.001f);

            //characterBackup.shootTimeSpaceRange = character.shootTimeSpaceRange;
            character.shootTimeSpaceRange = new Vector2(0.001f, 0.001f);

            //characterBackup.showHealthBar = character.showHealthBar;
            character.showHealthBar = true;

            //characterBackup.showName = character.showName;
            character.showName = true;

            //characterBackup.sightAngle = character.sightAngle;
            character.sightAngle = 360f;

            //character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            character.sightDistance *= 1.5f;
            
            return character;
        }
    }
}