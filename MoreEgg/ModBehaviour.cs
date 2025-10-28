using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Duckov.Economy;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using NodeCanvas.Tasks.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreEgg
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public Harmony harmony;

        public bool isFlag = false;
        public int windowID;
        public float GUIMultiply = 1f;

        public float sliderWidth;
        public float sliderHeight;

        public GUILayoutOption[] buttonOption;
        public GUILayoutOption[] lableOption;
        public GUILayoutOption[] textFieldOption;
        public GUILayoutOption[] sliderOption;

        public static float timer;
        public static int equipmentLevel = 6;
        public static Egg eggPrefab = null;

        public static Dictionary<int, SpawnCharacterInfo> spawnCharacterDict =
            new Dictionary<int, SpawnCharacterInfo>();

        public static Dictionary<string, int> spawnCharacterNameDict =
            new Dictionary<string, int>();

        public static List<int> characterCanUse = new List<int>();

        public static CharacterRandomPreset spawnCharacter = null;
        //public CharacterRandomPreset characterBackup = null;

        public static Dictionary<int, int> rifleGunDict = new Dictionary<int, int>();
        public static Dictionary<int, int> bulletDict = new Dictionary<int, int>();
        public static Dictionary<int, int> helmatDict = new Dictionary<int, int>();
        public static Dictionary<int, int> armorDict = new Dictionary<int, int>();
        public static Dictionary<int, int> backpackDict = new Dictionary<int, int>();

        public static bool isCheatAI = false;
        public static float aiHealthMultiply = 1;
        public static float aiDamageMultiply = 1;
        public static float aiSpeedMultiply = 1;
        public static float aiGunScatterMultiply = 1;
        public static float aiSightDistanceMultiply = 1;

        public static bool isBossAI = false;
        public static int bossID = 0;

        public static int aiPickupMoney = 0;
        public static float aiMoneyMultiply = 1;
        public static bool isExplosion = true;

        public static List<AIManager.AIDataStruct> aiDataList = new List<AIManager.AIDataStruct>();

        public static bool isCustomEquipment = false;

        public static int primaryWeaponID = 862;
        public static int bulletID = 694;
        public static int primaryWeaponScopeSlotID = 570;
        public static int primaryWeaponMuzzleSlotID = 482;
        public static int primaryWeaponGripSlotID = 453;
        public static int primaryWeaponStockSlotID = 508;
        public static int primaryWeaponTecSlotID = 576;
        public static int primaryWeaponMagSlotID = 548;

        //public static int secondaryWeaponID;
        public static int meleeWeaponID = 1176;
        public static int helmatID = 46;
        public static int armorID = 885;
        public static int faceMaskID = 26;
        public static int headsetID = 679;
        public static int backpackID = 40;
        public static int totem1ID = 959;
        public static int totem2ID = 964;

        public static string aiName = "护航";


        public class SpawnCharacterInfo
        {
            public int id;
            public string nameKey;
            public string name;
            public CharacterRandomPreset spawnCharacter;

            public SpawnCharacterInfo(int id, string nameKey, string name, CharacterRandomPreset spawnCharacter)
            {
                this.id = id;
                this.nameKey = nameKey;
                this.name = name;
                this.spawnCharacter = spawnCharacter;
            }
        }

        public Rect MyWindow { get; set; }

        private void Awake()
        {
            this.isFlag = false;
            this.windowID = new System.Random().Next();
            this.GUIMultiply = 1f;
            this.sliderWidth = 200 * this.GUIMultiply;
            this.sliderHeight = 20 * this.GUIMultiply;
            buttonOption = new GUILayoutOption[2]
                { GUILayout.Width(150f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
            lableOption = new GUILayoutOption[2]
                { GUILayout.Width(150f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
            textFieldOption = new GUILayoutOption[2]
                { GUILayout.Width(100f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
            sliderOption = new GUILayoutOption[2]
                { GUILayout.Width(this.sliderWidth), GUILayout.Height(this.sliderHeight) };
        }

        private void OnEnable()
        {
            HarmonyLoad.Load0Harmony();
        }

        private void OnDisable()
        {
            harmony.UnpatchAll("MoreEgg");
        }

        private void Start()
        {
            harmony = new Harmony("MoreEgg");
            harmony.PatchAll();
            this.isFlag = true;
            ModBehaviour.timer = 0;
            this.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                isFlag = !isFlag;
            }

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                try
                {
                    AIManager.AIGotoHere(ModBehaviour.GetGroundPosition(), 5);
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                try
                {
                    AIManager.AIRandomGotoHere(ModBehaviour.GetGroundPosition(), 5);
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse4) || Input.GetKeyDown(KeyCode.J))
            {
                try
                {
                    AIManager.SearchAndPickupAndHeal();
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse3) || Input.GetKeyDown(KeyCode.K))
            {
                try
                {
                    AIManager.DropItem();
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                try
                {
                    AIManager.AllReload();
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                try
                {
                    if (ModBehaviour.isExplosion)
                    {
                        this.StartCoroutine(CreateExplosion(ModBehaviour.GetGroundPosition()));
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }
            }

            this.TimeCountDown();
        }

        public void OnGUI()
        {
            if (!isFlag)
            {
                return;
            }

            MyWindow = GUILayout.Window(this.windowID, MyWindow, MyGUI, "来点护航MOD", new GUILayoutOption[0]);
        }

        public void CheatAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("红护开关", MyGUIStyleButton(ModBehaviour.isCheatAI), buttonOption))
            {
                ModBehaviour.isCheatAI = !ModBehaviour.isCheatAI;
            }

            GUILayout.Label("详细配置-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("HP倍率：" + ModBehaviour.aiHealthMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiHealthMultiply = GUILayout.HorizontalSlider(
                ModBehaviour.aiHealthMultiply,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                ModBehaviour.aiHealthMultiply = Mathf.Round(ModBehaviour.aiHealthMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.Label("伤害倍率：" + ModBehaviour.aiDamageMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiDamageMultiply = GUILayout.HorizontalSlider(
                ModBehaviour.aiDamageMultiply,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                ModBehaviour.aiDamageMultiply = Mathf.Round(ModBehaviour.aiDamageMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.Label("速度倍率：" + ModBehaviour.aiSpeedMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiSpeedMultiply = GUILayout.HorizontalSlider(
                ModBehaviour.aiSpeedMultiply,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                ModBehaviour.aiSpeedMultiply = Mathf.Round(ModBehaviour.aiSpeedMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("红护开关", MyGUIStyleButton(ModBehaviour.isCheatAI), buttonOption))
            {
                ModBehaviour.isCheatAI = !ModBehaviour.isCheatAI;
            }

            GUILayout.Label("详细配置-->", MyGUIStyleLable(), lableOption);

            GUILayout.Label("射击散布：" + ModBehaviour.aiGunScatterMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiGunScatterMultiply = GUILayout.HorizontalSlider(
                ModBehaviour.aiGunScatterMultiply,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                ModBehaviour.aiGunScatterMultiply = Mathf.Round(ModBehaviour.aiGunScatterMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.Label("视距倍率：" + ModBehaviour.aiSightDistanceMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiSightDistanceMultiply = GUILayout.HorizontalSlider(
                ModBehaviour.aiSightDistanceMultiply,
                0.1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                ModBehaviour.aiSightDistanceMultiply = Mathf.Round(ModBehaviour.aiSightDistanceMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void EquipmentLevelAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("装备等级", MyGUIStyleLable(), lableOption);

            GUILayout.Label("详细配置-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("装备等级：" + ModBehaviour.equipmentLevel, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.equipmentLevel = (int)GUILayout.HorizontalSlider(
                ModBehaviour.equipmentLevel,
                0f,
                6,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void BossAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("护航单位", MyGUIStyleLable(), lableOption);
            GUILayout.Label("除雇佣兵外", MyGUIStyleLable(), lableOption);
            GUILayout.Label("都有未知的BUG", MyGUIStyleLable(), lableOption);
            GUILayout.Label("关闭制式装备", MyGUIStyleLable(), lableOption);
            GUILayout.Label("使用BOSS装备", MyGUIStyleLable(), lableOption);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("制式装备", MyGUIStyleButton(!ModBehaviour.isBossAI), buttonOption))
            {
                ModBehaviour.isBossAI = !ModBehaviour.isBossAI;
            }

            GUILayout.Label("详细配置-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("种类：" + ModBehaviour.spawnCharacterDict[ModBehaviour.bossID].name, MyGUIStyleLable(),
                lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.bossID = (int)GUILayout.HorizontalSlider(
                ModBehaviour.bossID,
                0f,
                32,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void AIMoneyMultiplyGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("现金倍率调整", MyGUIStyleLable(), lableOption);
            GUILayout.Label("影响现金分档", MyGUIStyleLable(), lableOption);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("现金倍率", MyGUIStyleLable(), lableOption);

            GUILayout.Label("详细配置-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("现金倍率：" + ModBehaviour.aiMoneyMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiMoneyMultiply = GUILayout.HorizontalSlider(
                ModBehaviour.aiMoneyMultiply,
                0.1f,
                1,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (this.isFlag)
            {
                ModBehaviour.aiMoneyMultiply = Mathf.Round(ModBehaviour.aiMoneyMultiply / 0.1f) * 0.1f;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void EasyAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("下单护航", MyGUIStyleButton(false), buttonOption))
            {
                ModBehaviour.Spawn(
                    ModBehaviour.bossID,
                    0.001f,
                    ModBehaviour.equipmentLevel,
                    ModBehaviour.equipmentLevel,
                    false,
                    ModBehaviour.isBossAI,
                    ModBehaviour.isCheatAI,
                    ModBehaviour.isCustomEquipment
                );
            }

            GUILayout.Label("不消耗资源", MyGUIStyleLable(), lableOption);
            if (GUILayout.Button("护航自雷", MyGUIStyleButton(false), buttonOption))
            {
                this.KillAllAI();
            }

            if (GUILayout.Button("轰炸开关", MyGUIStyleButton(ModBehaviour.isExplosion), buttonOption))
            {
                ModBehaviour.isExplosion = !ModBehaviour.isExplosion;
            }

            GUILayout.EndHorizontal();
        }

        public void CustomEquipmentGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("自定义装备", MyGUIStyleButton(ModBehaviour.isCustomEquipment), buttonOption))
            {
                ModBehaviour.isCustomEquipment = !ModBehaviour.isCustomEquipment;
            }

            ModBehaviour.aiName =
                GUILayout.TextField(
                    aiName,
                    20,
                    MyGUIStyleTextField(),
                    textFieldOption
                );

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (!ModBehaviour.isCustomEquipment)
            {
                return;
            }


            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("第一行依次是-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("武器、子弹", MyGUIStyleLable(), lableOption);
            GUILayout.Label("瞄具、枪口", MyGUIStyleLable(), lableOption);
            GUILayout.Label("握把、枪托", MyGUIStyleLable(), lableOption);
            GUILayout.Label("战术、弹夹", MyGUIStyleLable(), lableOption);
            GUILayout.Label("", MyGUIStyleLable(), lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("第二行依次是-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("近战、头部", MyGUIStyleLable(), lableOption);
            GUILayout.Label("身体、面部", MyGUIStyleLable(), lableOption);
            GUILayout.Label("耳机、背包", MyGUIStyleLable(), lableOption);
            GUILayout.Label("图腾、图腾", MyGUIStyleLable(), lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            ModBehaviour.primaryWeaponID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.bulletID = Convert.ToInt32(
                GUILayout.TextField(
                    bulletID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.primaryWeaponScopeSlotID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponScopeSlotID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.primaryWeaponMuzzleSlotID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponMuzzleSlotID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.primaryWeaponGripSlotID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponGripSlotID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.primaryWeaponStockSlotID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponStockSlotID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.primaryWeaponTecSlotID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponTecSlotID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.primaryWeaponMagSlotID = Convert.ToInt32(
                GUILayout.TextField(
                    primaryWeaponMagSlotID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            ModBehaviour.meleeWeaponID = Convert.ToInt32(
                GUILayout.TextField(
                    meleeWeaponID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.helmatID = Convert.ToInt32(
                GUILayout.TextField(
                    helmatID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.armorID = Convert.ToInt32(
                GUILayout.TextField(
                    armorID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.faceMaskID = Convert.ToInt32(
                GUILayout.TextField(
                    faceMaskID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.headsetID = Convert.ToInt32(
                GUILayout.TextField(
                    headsetID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.backpackID = Convert.ToInt32(
                GUILayout.TextField(
                    backpackID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.totem1ID = Convert.ToInt32(
                GUILayout.TextField(
                    totem1ID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );
            ModBehaviour.totem2ID = Convert.ToInt32(
                GUILayout.TextField(
                    totem2ID.ToString(),
                    5,
                    MyGUIStyleTextField(),
                    textFieldOption
                )
            );

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
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("关闭界面", MyGUIStyleButton(false), buttonOption))
            {
                this.isFlag = false;
            }

            if (GUILayout.Button("放大界面", MyGUIStyleButton(false), buttonOption))
            {
                this.GUIMultiply *= 1.5f;
                this.sliderWidth = 200 * this.GUIMultiply;
                this.sliderHeight = 20 * this.GUIMultiply;
                buttonOption = new GUILayoutOption[2]
                    { GUILayout.Width(150f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
                lableOption = new GUILayoutOption[2]
                    { GUILayout.Width(150f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
                textFieldOption = new GUILayoutOption[2]
                    { GUILayout.Width(100f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
                sliderOption = new GUILayoutOption[2]
                    { GUILayout.Width(this.sliderWidth), GUILayout.Height(this.sliderHeight) };
            }

            if (GUILayout.Button("缩小界面", MyGUIStyleButton(false), buttonOption))
            {
                this.GUIMultiply /= 1.5f;
                this.sliderWidth = 200 * this.GUIMultiply;
                this.sliderHeight = 20 * this.GUIMultiply;
                buttonOption = new GUILayoutOption[2]
                    { GUILayout.Width(150f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
                lableOption = new GUILayoutOption[2]
                    { GUILayout.Width(150f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
                textFieldOption = new GUILayoutOption[2]
                    { GUILayout.Width(100f * this.GUIMultiply), GUILayout.Height(50f * this.GUIMultiply) };
                sliderOption = new GUILayoutOption[2]
                    { GUILayout.Width(this.sliderWidth), GUILayout.Height(this.sliderHeight) };
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("追求平衡和玩法，", MyGUIStyleLable(), lableOption);
            GUILayout.Label("不要开启红护", MyGUIStyleLable(), lableOption);
            GUILayout.Label("不要免费下单", MyGUIStyleLable(), lableOption);
            GUILayout.Label("关闭此页面", MyGUIStyleLable(), lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("左侧是功能总开关，", MyGUIStyleLable(), lableOption);
            GUILayout.Label("右侧是详细配置", MyGUIStyleLable(), lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            this.CheatAIGUI();
            this.EquipmentLevelAIGUI();
            this.BossAIGUI();
            this.AIMoneyMultiplyGUI();
            this.EasyAIGUI();

            this.CustomEquipmentGUI();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
        }

        public GUIStyle MyGUIStyleButton(bool flag)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = (int)(20 * this.GUIMultiply);
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
            style.fontSize = (int)(25 * this.GUIMultiply / 1.5);
            return style;
        }

        public GUIStyle MyGUIStyleTextField()
        {
            GUIStyle style = new GUIStyle(GUI.skin.textField);
            style.fontSize = (int)(25 * this.GUIMultiply / 1.5);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            return style;
        }

        public static void Spawn(int tempBossID, float waitTime, int tempEquipmentLevel, int weaponLevel,
            bool isRandom = true, bool isBoss = false, bool isCheat = false, bool isCustom = false)
        {
            if (isRandom)
            {
                tempBossID =
                    ModBehaviour.characterCanUse
                        [UnityEngine.Random.Range(0, ModBehaviour.characterCanUse.Count)];
            }

            try
            {
                SpawnEgg.Init();
            }
            catch (Exception e)
            {
                var a = e;
            }

            try
            {
                LevelManager.Instance.StartCoroutine(SpawnEgg.SpawnEggFunc(tempBossID, waitTime, 
                    tempEquipmentLevel, weaponLevel, isBoss, isCheat, isCustom));
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        public void KillAllAI()
        {
            AIManager.KillAllAI();
        }

        public IEnumerator CreateExplosion(Vector3 pos)
        {
            if (EconomyManager.Money < 20000)
            {
                yield break;
            }

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
                ai.CharacterMainControl.PopText("对地轰炸来袭");
            }

            yield return new WaitForSeconds(1);
            for (int i = 0; i < 5; i++)
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

            EconomyManager.Pay(new Cost(20000));
            yield break;
        }

        public static Vector3 GetGroundPosition()
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


        public void TimeCountDown()
        {
            if (ModBehaviour.timer >= 0)
            {
                ModBehaviour.timer -= Time.deltaTime;
            }
        }

        public void Init()
        {
            ModBehaviour.characterCanUse.Add(0);
            //ModBehaviour.characterCanUse.Add(1);
            ModBehaviour.characterCanUse.Add(2);
            ModBehaviour.characterCanUse.Add(3);
            ModBehaviour.characterCanUse.Add(4);
            ModBehaviour.characterCanUse.Add(5);
            ModBehaviour.characterCanUse.Add(6);
            ModBehaviour.characterCanUse.Add(7);
            ModBehaviour.characterCanUse.Add(8);
            //ModBehaviour.characterCanUse.Add(9);
            ModBehaviour.characterCanUse.Add(10);
            //ModBehaviour.characterCanUse.Add(11);
            ModBehaviour.characterCanUse.Add(12);
            ModBehaviour.characterCanUse.Add(13);
            //ModBehaviour.characterCanUse.Add(14);
            //ModBehaviour.characterCanUse.Add(15);
            ModBehaviour.characterCanUse.Add(16);
            ModBehaviour.characterCanUse.Add(17);
            ModBehaviour.characterCanUse.Add(18);
            //ModBehaviour.characterCanUse.Add(19);
            ModBehaviour.characterCanUse.Add(20);
            //ModBehaviour.characterCanUse.Add(21);
            //ModBehaviour.characterCanUse.Add(22);
            ModBehaviour.characterCanUse.Add(23);
            ModBehaviour.characterCanUse.Add(24);
            ModBehaviour.characterCanUse.Add(25);
            //ModBehaviour.characterCanUse.Add(26);
            ModBehaviour.characterCanUse.Add(27);
            //ModBehaviour.characterCanUse.Add(28);
            ModBehaviour.characterCanUse.Add(29);
            //ModBehaviour.characterCanUse.Add(30);
            //ModBehaviour.characterCanUse.Add(31);
            //ModBehaviour.characterCanUse.Add(32);


            ModBehaviour.spawnCharacterDict.Add(0, new SpawnCharacterInfo(0, "Cname_Usec", "雇佣兵", null));
            ModBehaviour.spawnCharacterDict.Add(1, new SpawnCharacterInfo(1, "Cname_SchoolBully", "校霸", null));
            ModBehaviour.spawnCharacterDict.Add(2, new SpawnCharacterInfo(2, "Cname_SpeedyChild", "急速团成员", null));
            ModBehaviour.spawnCharacterDict.Add(3, new SpawnCharacterInfo(3, "Cname_Speedy", "急速团长", null));
            ModBehaviour.spawnCharacterDict.Add(4, new SpawnCharacterInfo(4, "Cname_ShortEagle", "矮鸭", null));
            ModBehaviour.spawnCharacterDict.Add(5, new SpawnCharacterInfo(5, "Cname_Boss_Arcade", "暴走街机", null));
            ModBehaviour.spawnCharacterDict.Add(6, new SpawnCharacterInfo(6, "Cname_Vida", "维达", null));
            ModBehaviour.spawnCharacterDict.Add(7, new SpawnCharacterInfo(7, "Cname_BALeader", "BA队长", null));
            ModBehaviour.spawnCharacterDict.Add(8, new SpawnCharacterInfo(8, "Cname_Roadblock", "路障", null));
            ModBehaviour.spawnCharacterDict.Add(9, new SpawnCharacterInfo(9, "Cname_ScavRage", "暴走拾荒者", null));
            ModBehaviour.spawnCharacterDict.Add(10, new SpawnCharacterInfo(10, "Cname_ServerGuardian", "矿长", null));
            ModBehaviour.spawnCharacterDict.Add(11, new SpawnCharacterInfo(11, "Cname_CrazyRob", "失控机械蜘蛛", null));
            ModBehaviour.spawnCharacterDict.Add(12, new SpawnCharacterInfo(12, "Cname_Boss_Fly_Child", "蝇蝇队员", null));
            ModBehaviour.spawnCharacterDict.Add(13, new SpawnCharacterInfo(13, "Cname_Boss_Fly", "蝇蝇队长", null));
            ModBehaviour.spawnCharacterDict.Add(14, new SpawnCharacterInfo(14, "Cname_MonsterClimb", "风暴虫", null));
            ModBehaviour.spawnCharacterDict.Add(15, new SpawnCharacterInfo(15, "Cname_RobSpider", "机械蜘蛛", null));
            ModBehaviour.spawnCharacterDict.Add(16, new SpawnCharacterInfo(16, "Cname_Raider", "游荡者", null));
            ModBehaviour.spawnCharacterDict.Add(17, new SpawnCharacterInfo(17, "Cname_SenorEngineer", "高级工程师", null));
            ModBehaviour.spawnCharacterDict.Add(18, new SpawnCharacterInfo(18, "Cname_StormCreature", "风暴生物", null));
            ModBehaviour.spawnCharacterDict.Add(19, new SpawnCharacterInfo(19, "Cname_SchoolBully_Child", "校友", null));
            ModBehaviour.spawnCharacterDict.Add(20, new SpawnCharacterInfo(20, "Cname_Boss_3Shot", "三枪哥", null));
            ModBehaviour.spawnCharacterDict.Add(21, new SpawnCharacterInfo(21, "Cname_Grenade", "炸弹狂人", null));
            ModBehaviour.spawnCharacterDict.Add(22, new SpawnCharacterInfo(22, "Cname_Boss_Sniper", "劳登", null));
            ModBehaviour.spawnCharacterDict.Add(23, new SpawnCharacterInfo(23, "Cname_Scav", "拾荒者", null));
            ModBehaviour.spawnCharacterDict.Add(24, new SpawnCharacterInfo(24, "Cname_3Shot_Child", "三枪弟", null));
            ModBehaviour.spawnCharacterDict.Add(25, new SpawnCharacterInfo(25, "Cname_BALeader_Child", "普通BA", null));
            ModBehaviour.spawnCharacterDict.Add(26, new SpawnCharacterInfo(26, "Cname_StormBoss2", "咕噜咕噜", null));
            ModBehaviour.spawnCharacterDict.Add(27, new SpawnCharacterInfo(27, "Cname_StormVirus", "风暴？", null));
            ModBehaviour.spawnCharacterDict.Add(28, new SpawnCharacterInfo(28, "Cname_StormBoss5", "口口口口", null));
            ModBehaviour.spawnCharacterDict.Add(29, new SpawnCharacterInfo(29, "Cname_Boss_Shot", "喷子", null));
            ModBehaviour.spawnCharacterDict.Add(30, new SpawnCharacterInfo(30, "Cname_StormBoss1", "噗咙噗咙", null));
            ModBehaviour.spawnCharacterDict.Add(31, new SpawnCharacterInfo(31, "Cname_StormBoss4", "比利比利", null));
            ModBehaviour.spawnCharacterDict.Add(32, new SpawnCharacterInfo(32, "Cname_StormBoss3", "啪啦啪啦", null));


            ModBehaviour.spawnCharacterNameDict.Add("Cname_Usec", 0);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_SchoolBully", 1);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_SpeedyChild", 2);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Speedy", 3);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_ShortEagle", 4);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Arcade", 5);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Vida", 6);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_BALeader", 7);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Roadblock", 8);

            ModBehaviour.spawnCharacterNameDict.Add("Cname_ScavRage", 9);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_ServerGuardian", 10);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_CrazyRob", 11);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Fly_Child", 12);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Fly", 13);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_MonsterClimb", 14);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_RobSpider", 15);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Raider", 16);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_SenorEngineer", 17);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormCreature", 18);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_SchoolBully_Child", 19);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_3Shot", 20);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Grenade", 21);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Sniper", 22);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Scav", 23);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_3Shot_Child", 24);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_BALeader_Child", 25);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss2", 26);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormVirus", 27);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss5", 28);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Shot", 29);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss1", 30);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss4", 31);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss3", 32);

            //VPO-136
            ModBehaviour.rifleGunDict.Add(1, 659);
            ModBehaviour.rifleGunDict.Add(2, 659);
            //AK-47
            ModBehaviour.rifleGunDict.Add(3, 240);
            //AK-103
            ModBehaviour.rifleGunDict.Add(4, 238);
            //RPK
            ModBehaviour.rifleGunDict.Add(5, 244);
            //火麒麟
            ModBehaviour.rifleGunDict.Add(6, 862);

            //AR-生锈弹
            ModBehaviour.bulletDict.Add(1, 603);
            //AR-普通弹
            ModBehaviour.bulletDict.Add(2, 604);
            ModBehaviour.bulletDict.Add(3, 604);
            //AR-穿甲弹
            ModBehaviour.bulletDict.Add(4, 606);
            //AR-高级穿甲弹
            ModBehaviour.bulletDict.Add(5, 607);
            //AR-特种穿甲弹
            ModBehaviour.bulletDict.Add(6, 694);

            ModBehaviour.helmatDict.Add(1, 41);
            ModBehaviour.helmatDict.Add(2, 42);
            ModBehaviour.helmatDict.Add(3, 43);
            ModBehaviour.helmatDict.Add(4, 44);
            ModBehaviour.helmatDict.Add(5, 45);
            ModBehaviour.helmatDict.Add(6, 46);

            ModBehaviour.armorDict.Add(1, 32);
            ModBehaviour.armorDict.Add(2, 33);
            ModBehaviour.armorDict.Add(3, 2);
            ModBehaviour.armorDict.Add(4, 34);
            ModBehaviour.armorDict.Add(5, 35);
            ModBehaviour.armorDict.Add(6, 885);

            ModBehaviour.backpackDict.Add(1, 36);
            ModBehaviour.backpackDict.Add(2, 37);
            ModBehaviour.backpackDict.Add(3, 38);
            ModBehaviour.backpackDict.Add(4, 39);
            ModBehaviour.backpackDict.Add(5, 40);
            ModBehaviour.backpackDict.Add(6, 40);
        }
    }
}