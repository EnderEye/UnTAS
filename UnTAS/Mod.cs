using UnityEngine;
using MelonLoader;
using UnTAS;

[assembly: MelonInfo(typeof(Mod), "UnTAS", "0.0.1", "EnderEye")]

namespace UnTAS
{
    public class Mod : MelonMod
    {
        public static float prevTimeScale = 1f;
        public static float baseTimeScale = 1f;
        public static bool frozen = false;

        public static KeyCode SlowKey = KeyCode.Minus;
        public static KeyCode FastKey = KeyCode.Equals;
        public static KeyCode PauseKey = KeyCode.P;

        public static void DrawFrozenText()
        {
            GUI.Label(new Rect(20, 20, 1000, 200), "<b><color=white><size=100>Frozen</size></color></b>");
        }
        
        public static void DrawWatermark()
        {
            GUI.Label(new Rect(Screen.width - 225, Screen.height - 50, 1000, 200), "<b><color=white><size=30>UnTAS v0.0.1</size></color></b>");
        }

        public override void OnLateInitializeMelon()
        {
            MelonEvents.OnGUI.Subscribe(DrawWatermark, 101);
        }

        public override void OnUpdate()
        {
            if(!frozen)
            {
                if (Input.GetKeyDown(SlowKey))
                {
                    if (Time.timeScale > 0.1f)
                    {
                        baseTimeScale -= 0.1f;
                    }
                }

                if (Input.GetKeyDown(FastKey))
                {
                    baseTimeScale += 0.1f;
                }
            }

            if(Input.GetKeyDown(PauseKey))
            {
                ToggleFreeze();
            }

            Time.timeScale = baseTimeScale;
        }

        private static void ToggleFreeze()
        {
            frozen = !frozen;

            if (frozen)
            {
                MelonEvents.OnGUI.Subscribe(DrawFrozenText, 100);
                prevTimeScale = baseTimeScale;
                baseTimeScale = 0f;
            }
            else
            {
                MelonEvents.OnGUI.Unsubscribe(DrawFrozenText);
                baseTimeScale = prevTimeScale;
            }
        }

        public override void OnDeinitializeMelon()
        {
            if (frozen)
            {
                ToggleFreeze();
            }
        }
    }
}
