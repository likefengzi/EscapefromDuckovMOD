using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;

namespace MergeMyMOD
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public Harmony harmony;
        public bool isFlag = false;

        public int sliderWidth;
        public int sliderHeight;

        public GUILayoutOption[] buttonOption;
        public GUILayoutOption[] lableOption;
        public GUILayoutOption[] sliderOption;

        public class MyCustom
        {
            public static bool isMoreEnemy = false;
            public static bool isStrongerEnemy = false;
            public static bool isBetterEnemy = false;
            public static bool isMorePoints = true;
            public static int BossMultiply = 1;
            public static int EnemyMultiply = 1;

            public static bool isSuperDuck = false;
            public static float HealthPower = 2;
            public static float BasePower = 2;
            public static float WeightPower = 2;
            public static float SpeedPower = 2;
            public static float DamagePower = 2;
            public static float ProtectionPower = 2;

            public static bool isAutoHeal = false;
            public static float HealMultiply = 1;

            public static bool isWeakerEnemy = false;
            public static float EnemySearchAngleMultiply = 1;
            public static float EnemySearchDistanceMultiply = 1;
            
            public static bool isHighQualityItem = false;
            public static float HighQualityChanceMultiplier = 1;
            public static float ItemCountMultiplier = 1;

            public static bool isSuperPet = false;
            public static bool isSuperPet77 = false;

            public static bool isDeadNoDrop = false;
            public static bool isSaveItem = true;

            public static bool isNoFog = false;
            public static bool isBetterFishing = false;
            public static bool isMoreLootBox = false;
            public static bool isNoDurabilityLoss = false;
            public static bool isInfinityDurability = false;
            public static bool isInfinityBullet = false;
        }

        public Rect MyWindow { get; set; }

        private void Awake()
        {
            this.isFlag = false;
            this.sliderWidth = 200;
            this.sliderHeight = 20;
            buttonOption = new GUILayoutOption[2]
                { GUILayout.Width(150f), GUILayout.Height(50f) };
            lableOption = new GUILayoutOption[2]
                { GUILayout.Width(150f), GUILayout.Height(50f) };
            sliderOption = new GUILayoutOption[2]
                { GUILayout.Width(this.sliderWidth), GUILayout.Height(this.sliderHeight) };
        }

        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void OnDisable()
        {
            harmony.UnpatchAll("MergeMyMOD");
        }

        private void Start()
        {
            harmony = new Harmony("MergeMyMOD");
            harmony.PatchAll();
            this.isFlag = true;
            try
            {
                MyPlayerPrefs.Load();
            }
            catch (Exception e)
            {
                var a = e;
            }
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                isFlag = !isFlag;
                MyPlayerPrefs.Save();
            }

            this.CheatUpdate();
        }

        public void OnGUI()
        {
            if (!isFlag)
            {
                return;
            }

            MyWindow = GUILayout.Window(0, MyWindow, MyGUI, "MOD", new GUILayoutOption[0]);
        }

        public void MoreEnemyGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("更多敌人", MyGUIStyleButton(MyCustom.isMoreEnemy), buttonOption))
            {
                MyCustom.isMoreEnemy = !MyCustom.isMoreEnemy;
            }

            GUILayout.Label("详细配置-->", lableOption);
            if (GUILayout.Button("寻血猎犬", MyGUIStyleButton(MyCustom.isStrongerEnemy), buttonOption))
            {
                MyCustom.isStrongerEnemy = !MyCustom.isStrongerEnemy;
            }
            if (GUILayout.Button("超雄人机", MyGUIStyleButton(MyCustom.isBetterEnemy), buttonOption))
            {
                MyCustom.isBetterEnemy = !MyCustom.isBetterEnemy;
            }

            if (GUILayout.Button("激活刷新点", MyGUIStyleButton(MyCustom.isMorePoints), buttonOption))
            {
                MyCustom.isMorePoints = !MyCustom.isMorePoints;
            }

            GUILayout.Label("BOSS倍率：" + MyCustom.BossMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.BossMultiply = (int)GUILayout.HorizontalSlider(
                MyCustom.BossMultiply,
                0,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("敌人倍率：" + MyCustom.EnemyMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.EnemyMultiply = (int)GUILayout.HorizontalSlider(
                MyCustom.EnemyMultiply,
                0,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void SuperDuckGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("鸭科夫超级鸭", MyGUIStyleButton(MyCustom.isSuperDuck), buttonOption))
            {
                MyCustom.isSuperDuck = !MyCustom.isSuperDuck;
            }

            GUILayout.Label("详细配置-->", lableOption);
            GUILayout.Label("HP倍率：" + MyCustom.HealthPower, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.HealthPower = GUILayout.HorizontalSlider(
                MyCustom.HealthPower,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.HealthPower = Mathf.Round(MyCustom.HealthPower / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("基础数据倍率：" + MyCustom.BasePower, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.BasePower = GUILayout.HorizontalSlider(
                MyCustom.BasePower,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.BasePower = Mathf.Round(MyCustom.BasePower / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("负重倍率：" + MyCustom.WeightPower, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.WeightPower = GUILayout.HorizontalSlider(
                MyCustom.WeightPower,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.WeightPower = Mathf.Round(MyCustom.WeightPower / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            
            
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("鸭科夫超级鸭", MyGUIStyleButton(MyCustom.isSuperDuck), buttonOption))
            {
                MyCustom.isSuperDuck = !MyCustom.isSuperDuck;
            }

            GUILayout.Label("详细配置-->", lableOption);
            
            GUILayout.Label("速度倍率：" + MyCustom.SpeedPower, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.SpeedPower = GUILayout.HorizontalSlider(
                MyCustom.SpeedPower,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.SpeedPower = Mathf.Round(MyCustom.SpeedPower / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("伤害倍率：" + MyCustom.DamagePower, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.DamagePower = GUILayout.HorizontalSlider(
                MyCustom.DamagePower,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.DamagePower = Mathf.Round(MyCustom.DamagePower / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("风暴防护倍率：" + MyCustom.ProtectionPower, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.ProtectionPower = GUILayout.HorizontalSlider(
                MyCustom.ProtectionPower,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.ProtectionPower = Mathf.Round(MyCustom.ProtectionPower / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void AutoHealGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("自动回血", MyGUIStyleButton(MyCustom.isAutoHeal), buttonOption))
            {
                MyCustom.isAutoHeal = !MyCustom.isAutoHeal;
            }

            GUILayout.Label("详细配置-->", lableOption);

            GUILayout.Label("回血倍率：" + MyCustom.HealMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.HealMultiply = GUILayout.HorizontalSlider(
                MyCustom.HealMultiply,
                0.1f,
                50,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.HealMultiply = Mathf.Round(MyCustom.HealMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void WeakerEnemyGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("小聋瞎敌人", MyGUIStyleButton(MyCustom.isWeakerEnemy), buttonOption))
            {
                MyCustom.isWeakerEnemy = !MyCustom.isWeakerEnemy;
            }

            GUILayout.Label("详细配置-->", lableOption);

            GUILayout.Label("搜索视角倍率：" + MyCustom.EnemySearchAngleMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.EnemySearchAngleMultiply = GUILayout.HorizontalSlider(
                MyCustom.EnemySearchAngleMultiply,
                0.1f,
                1,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.EnemySearchAngleMultiply = Mathf.Round(MyCustom.EnemySearchAngleMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("搜索距离倍率：" + MyCustom.EnemySearchDistanceMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.EnemySearchDistanceMultiply = GUILayout.HorizontalSlider(
                MyCustom.EnemySearchDistanceMultiply,
                0.1f,
                1,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.EnemySearchDistanceMultiply = Mathf.Round(MyCustom.EnemySearchDistanceMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        public void HighQualityItemGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("高爆率", MyGUIStyleButton(MyCustom.isHighQualityItem), buttonOption))
            {
                MyCustom.isHighQualityItem = !MyCustom.isHighQualityItem;
            }

            GUILayout.Label("详细配置-->", lableOption);

            GUILayout.Label("稀有几率：" + MyCustom.HighQualityChanceMultiplier, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.HighQualityChanceMultiplier = GUILayout.HorizontalSlider(
                MyCustom.HighQualityChanceMultiplier,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                MyCustom.HighQualityChanceMultiplier = Mathf.Round(MyCustom.HighQualityChanceMultiplier / 0.1f) * 0.1f;
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("出货数量：" + MyCustom.ItemCountMultiplier, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            MyCustom.ItemCountMultiplier = (int)GUILayout.HorizontalSlider(
                MyCustom.ItemCountMultiplier,
                1f,
                50,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void DeadNoDropGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("死亡不掉落", MyGUIStyleButton(MyCustom.isDeadNoDrop), buttonOption))
            {
                MyCustom.isDeadNoDrop = !MyCustom.isDeadNoDrop;
            }

            GUILayout.Label("详细配置-->", lableOption);
            if (GUILayout.Button("物品不掉落", MyGUIStyleButton(MyCustom.isSaveItem), buttonOption))
            {
                MyCustom.isSaveItem = !MyCustom.isSaveItem;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void SuperPetGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("超级宠物", MyGUIStyleButton(MyCustom.isSuperPet), buttonOption))
            {
                MyCustom.isSuperPet = !MyCustom.isSuperPet;
            }

            GUILayout.Label("详细配置-->", lableOption);
            if (GUILayout.Button("7*7用不了", MyGUIStyleButton(false), buttonOption))
            {
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }


        public void EasyModGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("下面是简单功能", lableOption);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("去除迷雾", MyGUIStyleButton(MyCustom.isNoFog), buttonOption))
            {
                MyCustom.isNoFog = !MyCustom.isNoFog;
            }

            if (GUILayout.Button("简单钓鱼", MyGUIStyleButton(MyCustom.isBetterFishing), buttonOption))
            {
                MyCustom.isBetterFishing = !MyCustom.isBetterFishing;
            }

            if (GUILayout.Button("更多物资箱", MyGUIStyleButton(MyCustom.isMoreLootBox), buttonOption))
            {
                MyCustom.isMoreLootBox = !MyCustom.isMoreLootBox;
            }

            if (GUILayout.Button("耐久上限不减", MyGUIStyleButton(MyCustom.isNoDurabilityLoss), buttonOption))
            {
                MyCustom.isNoDurabilityLoss = !MyCustom.isNoDurabilityLoss;
            }

            if (GUILayout.Button("无限耐久", MyGUIStyleButton(MyCustom.isInfinityDurability), buttonOption))
            {
                MyCustom.isInfinityDurability = !MyCustom.isInfinityDurability;
            }
            
            if (GUILayout.Button("无限子弹", MyGUIStyleButton(MyCustom.isInfinityBullet), buttonOption))
            {
                MyCustom.isInfinityBullet = !MyCustom.isInfinityBullet;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void ButtonModGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("下面是按钮触发的功能", lableOption);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("传送回家", MyGUIStyleButton(false), buttonOption))
            {
                TeleportHome.TeleportStart();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void MyGUI(int windowID)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUI.skin.horizontalSlider.alignment = TextAnchor.MiddleLeft;
            GUI.skin.verticalSlider.alignment = TextAnchor.MiddleLeft;
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            if (GUILayout.Button("关闭界面", MyGUIStyleButton(false), buttonOption))
            {
                this.isFlag = false;
                MyPlayerPrefs.Save();
            }

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("左侧是功能总开关，", lableOption);
            GUILayout.Label("右侧是详细配置", lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            this.MoreEnemyGUI();
            this.SuperDuckGUI();
            this.AutoHealGUI();
            this.WeakerEnemyGUI();
            this.HighQualityItemGUI();
            this.DeadNoDropGUI();
            this.SuperPetGUI();
            

            this.EasyModGUI();
            this.ButtonModGUI();
            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
        }

        public GUIStyle MyGUIStyleButton(bool flag)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 20;
            if (flag)
            {
                style.normal.textColor = Color.red;
            }

            return style;
        }

        public GUIStyle MyGUIStyleSlider()
        {
            GUIStyle style = new GUIStyle(GUI.skin.horizontalSlider);
            style.alignment = TextAnchor.MiddleLeft;
            style.fixedHeight = this.sliderHeight;
            style.fixedWidth = this.sliderWidth;
            return style;
        }

        public GUIStyle MyGUIStyleSliderThumb()
        {
            GUIStyle thumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
            thumbStyle.fixedHeight = this.sliderHeight;
            return thumbStyle;
        }

        public GUIStyle MyGUIStyleLable()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 15;
            return style;
        }

        public void CheatUpdate()
        {
            AutoHeal.AutoHealCountDown();
        }
    }
}