using System;
using System.Collections.Generic;
using Duckov.Utilities;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoreEnemy
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        
        public Harmony harmony;
        public bool isFlag = false;
        public int windowID;

        public int sliderWidth;
        public int sliderHeight;

        public GUILayoutOption[] buttonOption;
        public GUILayoutOption[] lableOption;
        public GUILayoutOption[] sliderOption;
        
        
        public static  bool isMoreEnemy = false;
        public static bool isMorePoints = false;
        public static int BossMultiply = 1;
        public static int EnemyMultiply = 1;
        public static Dictionary<string, int> enemyMultiply = new Dictionary<string, int>();

        public Rect MyWindow { get; set; }

        private void Awake()
        {
            this.isFlag = false;
            this.windowID = new System.Random().Next();
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
            harmony.UnpatchAll("MoreEnemy");
        }

        private void Start()
        {
            harmony = new Harmony("MoreEnemy");
            harmony.PatchAll();
            this.isFlag = true;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                isFlag = !isFlag;
            }
        }
        
        public void OnGUI()
        {
            if (!isFlag)
            {
                return;
            }

            MyWindow = GUILayout.Window(1, MyWindow, MyGUI, "更多敌人MOD", new GUILayoutOption[0]);
        }

        public void MoreEnemyGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("更多敌人", MyGUIStyleButton(ModBehaviour.isMoreEnemy), buttonOption))
            {
                ModBehaviour.isMoreEnemy = !ModBehaviour.isMoreEnemy;
            }

            GUILayout.Label("详细配置-->", lableOption);
            if (GUILayout.Button("激活刷新点", MyGUIStyleButton(ModBehaviour.isMorePoints), buttonOption))
            {
                ModBehaviour.isMorePoints = !ModBehaviour.isMorePoints;
            }

            GUILayout.Label("BOSS倍率：" + ModBehaviour.BossMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            ModBehaviour.BossMultiply = (int)GUILayout.HorizontalSlider(
                ModBehaviour.BossMultiply,
                0,
                10,
                MyGUIStyleSlider(),
                MyGUIStyleSliderThumb(),
                sliderOption
            );
            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.Label("敌人倍率：" + ModBehaviour.EnemyMultiply, MyGUIStyleLable(), lableOption);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            ModBehaviour.EnemyMultiply = (int)GUILayout.HorizontalSlider(
                ModBehaviour.EnemyMultiply,
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
            }

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("左侧是功能总开关，", lableOption);
            GUILayout.Label("右侧是详细配置", lableOption);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            this.MoreEnemyGUI();
            
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

        //每个点位都刷新敌人
        [HarmonyPatch(typeof(RandomCharacterSpawner), "CreateAsync")]
        public class EnemyIsAll
        {
            [HarmonyPrefix]
            static void Prefix(RandomCharacterSpawner __instance)
            {
                if (!ModBehaviour.isMoreEnemy)
                {
                    return;
                }

                if (ModBehaviour.isMorePoints)
                {
                    __instance.spawnCountRange =
                        new Vector2Int(__instance.spawnPoints.points.Count,
                            __instance.spawnPoints.points.Count);
                }
            }
        }

        //更多敌人
        [HarmonyPatch(typeof(RandomCharacterSpawner), nameof(RandomCharacterSpawner.StartSpawn))]
        public class EnemyIsMore
        {
            public static int num;

            [HarmonyPrefix]
            static bool Prefix(RandomCharacterSpawner __instance)
            {
                if (!ModBehaviour.isMoreEnemy)
                {
                    return true;
                }

                if (num > 0)
                {
                    num--;
                    return true;
                }

                if (__instance.masterGroup && !__instance.masterGroup.hasLeader)
                {
                    num = ModBehaviour.BossMultiply;
                }
                else
                {
                    num = ModBehaviour.EnemyMultiply;
                }

                return false;
            }

            [HarmonyPostfix]
            static void Postfix(RandomCharacterSpawner __instance)
            {
                if (!ModBehaviour.isMoreEnemy)
                {
                    return;
                }

                if (num > 0)
                {
                    __instance.StartSpawn();
                }
            }
        }
    }
}