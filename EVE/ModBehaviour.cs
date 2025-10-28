using System;
using System.Collections.Generic;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EVE
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

        public static bool isNoMove = false;
        public static bool isControlBoss = false;

        public static bool isCheatAI = false;
        public static float aiHealthMultiply = 1;
        public static float aiDamageMultiply = 1;
        public static float aiSpeedMultiply = 1;
        public static float aiGunScatterMultiply = 1;
        public static float aiSightDistanceMultiply = 1;

        public static bool isBossEquipment = true;
        public static int bossID = 0;
        public static int lastBossID = -1;
        public static int aiTeam = 0;
        public static int lastAITeam = -1;

        public static Dictionary<int, Teams> aiTeamDict =
            new Dictionary<int, Teams>();

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

        public static int spawnNum = 1;

        public static Egg eggPrefab = null;

        public static Dictionary<int, SpawnCharacterInfo> spawnCharacterDict =
            new Dictionary<int, SpawnCharacterInfo>();

        public static Dictionary<string, int> spawnCharacterNameDict =
            new Dictionary<string, int>();

        public static List<int> characterCanUse = new List<int>();
        public static float timer;


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
            harmony.UnpatchAll("EVE");
        }

        private void Start()
        {
            harmony = new Harmony("EVE");
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

            if (ModBehaviour.isControlBoss&&Input.GetKeyDown(KeyCode.Mouse2))
            {
                AIManager.MoveAllAI(ModBehaviour.GetGroundPosition(),5);
            }

            this.TimeCountDown();
        }

        public void OnGUI()
        {
            if (!isFlag)
            {
                return;
            }

            MyWindow = GUILayout.Window(this.windowID, MyWindow, MyGUI, "斗蛐蛐MOD", new GUILayoutOption[0]);
        }

        public void CheatAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("基础属性", MyGUIStyleLable(), lableOption);

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
            GUILayout.Label("基础属性", MyGUIStyleLable(), lableOption);

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


        public void BossAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("这些会攻击玩家", MyGUIStyleLable(), lableOption);
            GUILayout.Label("劳登、口口口口", MyGUIStyleLable(), lableOption);
            GUILayout.Label("咕噜咕噜、噗咙噗咙", MyGUIStyleLable(), lableOption);
            GUILayout.Label("比利比利、啪啦啪啦", MyGUIStyleLable(), lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("BOSS物种", MyGUIStyleLable(), lableOption);
            GUILayout.Label("详细配置-->", MyGUIStyleLable(), lableOption);
            GUILayout.Label("种类：" + ModBehaviour.spawnCharacterDict[ModBehaviour.bossID].name, MyGUIStyleLable(),
                lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.bossID = (int)GUILayout.HorizontalSlider(
                ModBehaviour.bossID,
                0f,
                37,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (ModBehaviour.bossID != ModBehaviour.lastBossID)
            {
                ModBehaviour.lastBossID = ModBehaviour.bossID;
                ModBehaviour.aiName = ModBehaviour.aiTeamDict[ModBehaviour.aiTeam] +
                                      ModBehaviour.spawnCharacterDict[ModBehaviour.bossID].name;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();
            GUILayout.Label("阵营：" + ModBehaviour.aiTeamDict[ModBehaviour.aiTeam], MyGUIStyleLable(),
                lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.aiTeam = (int)GUILayout.HorizontalSlider(
                ModBehaviour.aiTeam,
                0f,
                5,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            if (ModBehaviour.aiTeam != ModBehaviour.lastAITeam)
            {
                ModBehaviour.lastAITeam = ModBehaviour.aiTeam;
                ModBehaviour.aiName = ModBehaviour.aiTeamDict[ModBehaviour.aiTeam] +
                                      ModBehaviour.spawnCharacterDict[ModBehaviour.bossID].name;
            }

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }


        public void EasyAIGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("生成单位", MyGUIStyleButton(false), buttonOption))
            {
                ModBehaviour.Spawn(ModBehaviour.bossID, ModBehaviour.spawnNum, ModBehaviour.isBossEquipment);
            }
            GUILayout.Label("自定义名字", MyGUIStyleLable(), lableOption);
            ModBehaviour.aiName =
                GUILayout.TextField(
                    aiName,
                    20,
                    MyGUIStyleTextField(),
                    textFieldOption
                );

            GUILayout.Label("生成数量：" + ModBehaviour.spawnNum, MyGUIStyleLable(),
                lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20 * this.GUIMultiply);
            ModBehaviour.spawnNum = (int)GUILayout.HorizontalSlider(
                ModBehaviour.spawnNum,
                1f,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );

            GUILayout.Space(10 * this.GUIMultiply);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("一键清图", MyGUIStyleButton(false), buttonOption))
            {
                AIManager.KillAllAI();
            }


            GUILayout.EndHorizontal();
        }

        public void CustomEquipmentGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("自定义装备", MyGUIStyleButton(!ModBehaviour.isBossEquipment), buttonOption))
            {
                ModBehaviour.isBossEquipment = !ModBehaviour.isBossEquipment;
            }

            

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (ModBehaviour.isBossEquipment)
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
            // if (GUILayout.Button("和平模式", MyGUIStyleButton(ModBehaviour.isNoWar), buttonOption))
            // {
            //     ModBehaviour.isNoWar = !ModBehaviour.isNoWar;
            // }
            if (GUILayout.Button("开始斗蛐蛐", MyGUIStyleButton(false), buttonOption))
            {
                AIManager.ChangeAllTeam();
            }

            if (GUILayout.Button("禁止移动", MyGUIStyleButton(ModBehaviour.isNoMove), buttonOption))
            {
                ModBehaviour.isNoMove = !ModBehaviour.isNoMove;
            }

            if (GUILayout.Button("指挥全场", MyGUIStyleButton(ModBehaviour.isControlBoss), buttonOption))
            {
                ModBehaviour.isControlBoss = !ModBehaviour.isControlBoss;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            this.CheatAIGUI();
            this.BossAIGUI();
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

        public void TimeCountDown()
        {
            if (ModBehaviour.timer >= 0)
            {
                ModBehaviour.timer -= Time.deltaTime;
            }
        }

        public static void Spawn(int tempBossID, int num, bool isBoss = false)
        {
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
                LevelManager.Instance.StartCoroutine(SpawnEgg.SpawnEggFunc(tempBossID, num, isBoss));
            }
            catch (Exception e)
            {
                var a = e;
            }
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

        public void Init()
        {
            //ModBehaviour.aiTeamDict.Add(0, Teams.player);
            ModBehaviour.aiTeamDict.Add(0, Teams.scav);
            ModBehaviour.aiTeamDict.Add(1, Teams.usec);
            ModBehaviour.aiTeamDict.Add(2, Teams.bear);
            ModBehaviour.aiTeamDict.Add(3, Teams.middle);
            ModBehaviour.aiTeamDict.Add(4, Teams.lab);
            //ModBehaviour.aiTeamDict.Add(6, Teams.all);
            ModBehaviour.aiTeamDict.Add(5, Teams.wolf);

            ModBehaviour.characterCanUse.Add(0);
            ModBehaviour.characterCanUse.Add(1);
            ModBehaviour.characterCanUse.Add(2);
            ModBehaviour.characterCanUse.Add(3);
            ModBehaviour.characterCanUse.Add(4);
            ModBehaviour.characterCanUse.Add(5);
            ModBehaviour.characterCanUse.Add(6);
            ModBehaviour.characterCanUse.Add(7);
            ModBehaviour.characterCanUse.Add(8);
            ModBehaviour.characterCanUse.Add(9);
            ModBehaviour.characterCanUse.Add(10);
            ModBehaviour.characterCanUse.Add(11);
            ModBehaviour.characterCanUse.Add(12);
            ModBehaviour.characterCanUse.Add(13);
            ModBehaviour.characterCanUse.Add(14);
            ModBehaviour.characterCanUse.Add(15);
            ModBehaviour.characterCanUse.Add(16);
            ModBehaviour.characterCanUse.Add(17);
            ModBehaviour.characterCanUse.Add(18);
            ModBehaviour.characterCanUse.Add(19);
            ModBehaviour.characterCanUse.Add(20);
            ModBehaviour.characterCanUse.Add(21);
            ModBehaviour.characterCanUse.Add(22);
            ModBehaviour.characterCanUse.Add(23);
            ModBehaviour.characterCanUse.Add(24);
            ModBehaviour.characterCanUse.Add(25);
            ModBehaviour.characterCanUse.Add(26);
            ModBehaviour.characterCanUse.Add(27);
            ModBehaviour.characterCanUse.Add(28);
            ModBehaviour.characterCanUse.Add(29);
            ModBehaviour.characterCanUse.Add(30);
            ModBehaviour.characterCanUse.Add(31);
            ModBehaviour.characterCanUse.Add(32);
            ModBehaviour.characterCanUse.Add(33);
            ModBehaviour.characterCanUse.Add(34);
            ModBehaviour.characterCanUse.Add(35);
            ModBehaviour.characterCanUse.Add(36);
            ModBehaviour.characterCanUse.Add(37);

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
            ModBehaviour.spawnCharacterDict.Add(14, new SpawnCharacterInfo(14, "Character_Fo", "佛哥", null));
            ModBehaviour.spawnCharacterDict.Add(15, new SpawnCharacterInfo(15, "Cname_MonsterClimb", "风暴虫", null));
            ModBehaviour.spawnCharacterDict.Add(16, new SpawnCharacterInfo(16, "Cname_RobSpider", "机械蜘蛛", null));
            ModBehaviour.spawnCharacterDict.Add(17, new SpawnCharacterInfo(17, "Cname_Raider", "游荡者", null));
            ModBehaviour.spawnCharacterDict.Add(18, new SpawnCharacterInfo(18, "Cname_SenorEngineer", "高级工程师", null));
            ModBehaviour.spawnCharacterDict.Add(19, new SpawnCharacterInfo(19, "Cname_StormCreature", "风暴生物", null));
            ModBehaviour.spawnCharacterDict.Add(20, new SpawnCharacterInfo(20, "Cname_SchoolBully_Child", "校友", null));
            ModBehaviour.spawnCharacterDict.Add(21, new SpawnCharacterInfo(21, "Cname_Boss_3Shot", "三枪哥", null));
            ModBehaviour.spawnCharacterDict.Add(22, new SpawnCharacterInfo(22, "Cname_Grenade", "炸弹狂人", null));
            ModBehaviour.spawnCharacterDict.Add(23, new SpawnCharacterInfo(23, "Cname_Boss_Sniper", "劳登", null));
            ModBehaviour.spawnCharacterDict.Add(24, new SpawnCharacterInfo(24, "Cname_Scav", "拾荒者", null));
            ModBehaviour.spawnCharacterDict.Add(25, new SpawnCharacterInfo(25, "Cname_3Shot_Child", "三枪弟", null));
            ModBehaviour.spawnCharacterDict.Add(26, new SpawnCharacterInfo(26, "Cname_BALeader_Child", "普通BA", null));
            ModBehaviour.spawnCharacterDict.Add(27, new SpawnCharacterInfo(27, "Cname_StormBoss2", "咕噜咕噜", null));
            ModBehaviour.spawnCharacterDict.Add(28, new SpawnCharacterInfo(28, "Cname_StormVirus", "风暴？", null));
            ModBehaviour.spawnCharacterDict.Add(29, new SpawnCharacterInfo(29, "Cname_StormBoss5", "口口口口", null));
            ModBehaviour.spawnCharacterDict.Add(30, new SpawnCharacterInfo(30, "Cname_Boss_Shot", "喷子", null));
            ModBehaviour.spawnCharacterDict.Add(31, new SpawnCharacterInfo(31, "Cname_StormBoss1", "噗咙噗咙", null));
            ModBehaviour.spawnCharacterDict.Add(32, new SpawnCharacterInfo(32, "Cname_StormBoss4", "比利比利", null));
            ModBehaviour.spawnCharacterDict.Add(33, new SpawnCharacterInfo(33, "Cname_StormBoss3", "啪啦啪啦", null));
            ModBehaviour.spawnCharacterDict.Add(34, new SpawnCharacterInfo(34, "Character_Ming", "小明", null));
            ModBehaviour.spawnCharacterDict.Add(35, new SpawnCharacterInfo(35, "Cname_Wolf", "狼", null));
            ModBehaviour.spawnCharacterDict.Add(36, new SpawnCharacterInfo(36, "Cname_LabTestObjective", "测试对象", null));
            ModBehaviour.spawnCharacterDict.Add(37, new SpawnCharacterInfo(37, "Cname_Mushroom", "行走菇", null));

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
            ModBehaviour.spawnCharacterNameDict.Add("Character_Fo", 14);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_MonsterClimb", 15);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_RobSpider", 16);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Raider", 17);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_SenorEngineer", 18);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormCreature", 19);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_SchoolBully_Child", 20);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_3Shot", 21);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Grenade", 22);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Sniper", 23);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Scav", 24);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_3Shot_Child", 25);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_BALeader_Child", 26);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss2", 27);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormVirus", 28);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss5", 29);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Boss_Shot", 30);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss1", 31);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss4", 32);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_StormBoss3", 33);
            ModBehaviour.spawnCharacterNameDict.Add("Character_Ming", 34);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Wolf", 35);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_LabTestObjective", 36);
            ModBehaviour.spawnCharacterNameDict.Add("Cname_Mushroom", 37);
        }
    }
}