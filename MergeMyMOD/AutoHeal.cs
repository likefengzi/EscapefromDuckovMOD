using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MergeMyMOD
{
    public class AutoHeal
    {
        public static float timer;
        public static float timeInterval = 0.5f;

        public static void AutoHealCountDown()
        {
            if (AutoHeal.timer >= AutoHeal.timeInterval)
            {
                AutoHeal.timer = AutoHeal.timeInterval;
            }

            if (AutoHeal.timer <= AutoHeal.timeInterval)
            {
                AutoHeal.timer -= Time.deltaTime;
            }

            if (AutoHeal.timer <= 0)
            {
                AutoHeal.timer = AutoHeal.timeInterval;
                try
                {
                    AutoHeal.AutoHealAddHP();
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }
        }

        public static void AutoHealAddHP()
        {
            if (LevelManager.Instance.MainCharacter.Health.IsDead)
            {
                return;
            }

            if (LevelManager.Instance.MainCharacter.Health.CurrentHealth /
                LevelManager.Instance.MainCharacter.Health.MaxHealth < 0.5f)
            {
                LevelManager.Instance.MainCharacter.Health.AddHealth(
                    Random.Range(0.1f, 0.75f) * ModBehaviour.MyCustom.HealMultiply
                );
            }

            if (LevelManager.Instance.MainCharacter.Health.CurrentHealth /
                LevelManager.Instance.MainCharacter.Health.MaxHealth < 0.75f)
            {
                LevelManager.Instance.MainCharacter.Health.AddHealth(
                    Random.Range(0.1f, 0.25f) * ModBehaviour.MyCustom.HealMultiply
                );
            }
        }
    }
}