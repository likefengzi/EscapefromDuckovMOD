using System;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AutoSlowHeal
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public float timer;
        public float timeInterval = 0.5f;
        public static float healMultiply;

        private void Start()
        {
            ModBehaviour.healMultiply = LoadData.LoadDataFromFile(1f);
        }

        private void Update()
        {
            if (this.timer >= this.timeInterval)
            {
                this.timer = this.timeInterval;
            }

            if (this.timer <= this.timeInterval)
            {
                this.timer -= Time.deltaTime;
            }

            if (this.timer <= 0)
            {
                this.timer = this.timeInterval;
                try
                {
                    this.AutoHeal();
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public void AutoHeal()
        {
            if (LevelManager.Instance.MainCharacter.Health.IsDead)
            {
                return;
            }

            if (LevelManager.Instance.MainCharacter.Health.CurrentHealth /
                LevelManager.Instance.MainCharacter.Health.MaxHealth < 0.5f)
            {
                LevelManager.Instance.MainCharacter.Health.AddHealth(
                    Random.Range(0.1f, 0.75f) * ModBehaviour.healMultiply
                );
            }

            if (LevelManager.Instance.MainCharacter.Health.CurrentHealth /
                LevelManager.Instance.MainCharacter.Health.MaxHealth < 0.75f)
            {
                LevelManager.Instance.MainCharacter.Health.AddHealth(
                    Random.Range(0.1f, 0.25f) * ModBehaviour.healMultiply
                );
            }
        }
    }
}