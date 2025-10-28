using UnityEngine;

namespace MergeMyMOD
{
    public class MyPlayerPrefs
    {
        public static void Save()
        {
            PlayerPrefs.SetInt("MyCustom.isMoreEnemy", ModBehaviour.MyCustom.isMoreEnemy ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isStrongerEnemy", ModBehaviour.MyCustom.isStrongerEnemy ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isBetterEnemy", ModBehaviour.MyCustom.isBetterEnemy ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isMorePoints", ModBehaviour.MyCustom.isMorePoints ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.BossMultiply", ModBehaviour.MyCustom.BossMultiply);
            PlayerPrefs.SetInt("MyCustom.EnemyMultiply", ModBehaviour.MyCustom.EnemyMultiply);

            PlayerPrefs.SetInt("MyCustom.isSuperDuck", ModBehaviour.MyCustom.isSuperDuck ? 1 : 0);
            PlayerPrefs.SetFloat("MyCustom.HealthPower", ModBehaviour.MyCustom.HealthPower);
            PlayerPrefs.SetFloat("MyCustom.BasePower", ModBehaviour.MyCustom.BasePower);
            PlayerPrefs.SetFloat("MyCustom.WeightPower", ModBehaviour.MyCustom.WeightPower);
            PlayerPrefs.SetFloat("MyCustom.SpeedPower", ModBehaviour.MyCustom.SpeedPower);
            PlayerPrefs.SetFloat("MyCustom.DamagePower", ModBehaviour.MyCustom.DamagePower);
            PlayerPrefs.SetFloat("MyCustom.ProtectionPower", ModBehaviour.MyCustom.ProtectionPower);

            PlayerPrefs.SetInt("MyCustom.isAutoHeal", ModBehaviour.MyCustom.isAutoHeal ? 1 : 0);
            PlayerPrefs.SetFloat("MyCustom.HealMultiply", ModBehaviour.MyCustom.HealMultiply);

            PlayerPrefs.SetInt("MyCustom.isWeakerEnemy", ModBehaviour.MyCustom.isWeakerEnemy ? 1 : 0);
            PlayerPrefs.SetFloat("MyCustom.EnemySearchAngleMultiply", ModBehaviour.MyCustom.EnemySearchAngleMultiply);
            PlayerPrefs.SetFloat("MyCustom.EnemySearchDistanceMultiply",
                ModBehaviour.MyCustom.EnemySearchDistanceMultiply);
            
            PlayerPrefs.SetInt("MyCustom.isHighQualityItem", ModBehaviour.MyCustom.isHighQualityItem ? 1 : 0);
            PlayerPrefs.SetFloat("MyCustom.HighQualityChanceMultiplier", ModBehaviour.MyCustom.HighQualityChanceMultiplier);
            PlayerPrefs.SetFloat("MyCustom.ItemCountMultiplier",
                ModBehaviour.MyCustom.ItemCountMultiplier);

            PlayerPrefs.SetInt("MyCustom.isSuperPet", ModBehaviour.MyCustom.isSuperPet ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isSuperPet77", ModBehaviour.MyCustom.isSuperPet77 ? 1 : 0);

            PlayerPrefs.SetInt("MyCustom.isDeadNoDrop", ModBehaviour.MyCustom.isDeadNoDrop ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isSaveItem", ModBehaviour.MyCustom.isSaveItem ? 1 : 0);

            PlayerPrefs.SetInt("MyCustom.isNoFog", ModBehaviour.MyCustom.isNoFog ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isBetterFishing", ModBehaviour.MyCustom.isBetterFishing ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isMoreLootBox", ModBehaviour.MyCustom.isMoreLootBox ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isNoDurabilityLoss", ModBehaviour.MyCustom.isNoDurabilityLoss ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isInfinityDurability", ModBehaviour.MyCustom.isInfinityDurability ? 1 : 0);
            PlayerPrefs.SetInt("MyCustom.isInfinityBullet", ModBehaviour.MyCustom.isInfinityBullet ? 1 : 0);

            
            PlayerPrefs.Save();
        }

        public static void Load()
        {
            ModBehaviour.MyCustom.isMoreEnemy = PlayerPrefs.GetInt("MyCustom.isMoreEnemy", 0) == 1;
            ModBehaviour.MyCustom.isStrongerEnemy = PlayerPrefs.GetInt("MyCustom.isStrongerEnemy", 0) == 1;
            ModBehaviour.MyCustom.isBetterEnemy = PlayerPrefs.GetInt("MyCustom.isBetterEnemy", 0) == 1;
            ModBehaviour.MyCustom.isMorePoints = PlayerPrefs.GetInt("MyCustom.isMorePoints", 0) == 1;
            ModBehaviour.MyCustom.BossMultiply = PlayerPrefs.GetInt("MyCustom.BossMultiply", 1);
            ModBehaviour.MyCustom.EnemyMultiply = PlayerPrefs.GetInt("MyCustom.EnemyMultiply", 1);

            ModBehaviour.MyCustom.isSuperDuck = PlayerPrefs.GetInt("MyCustom.isSuperDuck", 0) == 1;
            ModBehaviour.MyCustom.HealthPower = PlayerPrefs.GetFloat("MyCustom.HealthPower", 1.0f);
            ModBehaviour.MyCustom.BasePower = PlayerPrefs.GetFloat("MyCustom.BasePower", 1.0f);
            ModBehaviour.MyCustom.WeightPower = PlayerPrefs.GetFloat("MyCustom.WeightPower", 1.0f);
            ModBehaviour.MyCustom.SpeedPower = PlayerPrefs.GetFloat("MyCustom.SpeedPower", 1.0f);
            ModBehaviour.MyCustom.DamagePower = PlayerPrefs.GetFloat("MyCustom.DamagePower", 1.0f);
            ModBehaviour.MyCustom.ProtectionPower = PlayerPrefs.GetFloat("MyCustom.ProtectionPower", 1.0f);

            ModBehaviour.MyCustom.isAutoHeal = PlayerPrefs.GetInt("MyCustom.isAutoHeal", 0) == 1;
            ModBehaviour.MyCustom.HealMultiply = PlayerPrefs.GetFloat("MyCustom.HealMultiply", 1.0f);

            ModBehaviour.MyCustom.isWeakerEnemy = PlayerPrefs.GetInt("MyCustom.isWeakerEnemy", 0) == 1;
            ModBehaviour.MyCustom.EnemySearchAngleMultiply =
                PlayerPrefs.GetFloat("MyCustom.EnemySearchAngleMultiply", 1.0f);
            ModBehaviour.MyCustom.EnemySearchDistanceMultiply =
                PlayerPrefs.GetFloat("MyCustom.EnemySearchDistanceMultiply", 1.0f);
            
            ModBehaviour.MyCustom.isHighQualityItem = PlayerPrefs.GetInt("MyCustom.isHighQualityItem", 0) == 1;
            ModBehaviour.MyCustom.HighQualityChanceMultiplier =
                PlayerPrefs.GetFloat("MyCustom.HighQualityChanceMultiplier", 1.0f);
            ModBehaviour.MyCustom.ItemCountMultiplier =
                PlayerPrefs.GetFloat("MyCustom.ItemCountMultiplier", 1.0f);

            ModBehaviour.MyCustom.isSuperPet = PlayerPrefs.GetInt("MyCustom.isSuperPet", 0) == 1;
            ModBehaviour.MyCustom.isSuperPet77 = PlayerPrefs.GetInt("MyCustom.isSuperPet77", 0) == 1;

            ModBehaviour.MyCustom.isDeadNoDrop = PlayerPrefs.GetInt("MyCustom.isDeadNoDrop", 0) == 1;
            ModBehaviour.MyCustom.isSaveItem = PlayerPrefs.GetInt("MyCustom.isSaveItem", 0) == 1;

            ModBehaviour.MyCustom.isNoFog = PlayerPrefs.GetInt("MyCustom.isNoFog", 0) == 1;
            ModBehaviour.MyCustom.isBetterFishing = PlayerPrefs.GetInt("MyCustom.isBetterFishing", 0) == 1;
            ModBehaviour.MyCustom.isMoreLootBox = PlayerPrefs.GetInt("MyCustom.isMoreLootBox", 0) == 1;
            ModBehaviour.MyCustom.isNoDurabilityLoss = PlayerPrefs.GetInt("MyCustom.isNoDurabilityLoss", 0) == 1;
            ModBehaviour.MyCustom.isInfinityDurability = PlayerPrefs.GetInt("MyCustom.isInfinityDurability", 0) == 1;
            ModBehaviour.MyCustom.isInfinityBullet = PlayerPrefs.GetInt("MyCustom.isInfinityBullet", 0) == 1;

        }
    }
}