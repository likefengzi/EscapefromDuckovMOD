using System;
using System.Collections;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace EVE
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

        public static IEnumerator SpawnEggFunc(int tempBossID, int num, bool isBoss)
        {
            yield return new WaitForSeconds(0.001f);
            CharacterRandomPreset spawnCharacter = ModBehaviour.spawnCharacterDict[tempBossID].spawnCharacter;
            //this.characterBackup = ScriptableObject.CreateInstance<CharacterRandomPreset>();
            spawnCharacter = SpawnEgg.SetUsec(spawnCharacter);

            for (int i = 0; i < num; i++)
            {
                Vector3 vector3 = new Vector3(UnityEngine.Random.Range(-2,2),2,UnityEngine.Random.Range(-2,2));
                Egg egg = UnityEngine.Object.Instantiate<Egg>(ModBehaviour.eggPrefab,
                    LevelManager.Instance.MainCharacter.transform.position+vector3, Quaternion.identity);

                egg.Init(LevelManager.Instance.MainCharacter.transform.position+vector3,
                    LevelManager.Instance.MainCharacter.CurrentAimDirection * 1f,
                    LevelManager.Instance.MainCharacter, spawnCharacter, 0.001f);
            }


            //StartCoroutine(this.LoadUsecAsync(this.spawnCharacter, this.characterBackup));

            LevelManager.Instance.MainCharacter.StartCoroutine(AIManager.ChangeAllEquipment(isBoss));

            LevelManager.Instance.MainCharacter.StartCoroutine(AIManager.MarkAllTeamAndLeader());

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
            character.health *=  ModBehaviour.aiHealthMultiply;

            //characterBackup.hearingAbility = character.hearingAbility;
            character.hearingAbility = 0.001f;

            character.moveSpeedFactor *= 1.5f * ModBehaviour.aiSpeedMultiply;

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

            character.sightDistance *= ModBehaviour.aiSightDistanceMultiply;
            return character;
        }
    }
}