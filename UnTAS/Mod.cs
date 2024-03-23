using UnityEngine;
using MelonLoader;
using UnTAS;
using System.Net;

[assembly: MelonInfo(typeof(Mod), "UnTAS", "0.0.2", "EnderEye")]

namespace UnTAS
{
    public class Mod : MelonMod
    {
        public static float prevTimeScale = 1f;
        public static float baseTimeScale = 1f;
        public static bool frozen = false;

        public static string version = "0.0.2";
        public static bool updated;

        public static KeyCode SlowKey = KeyCode.Minus;
        public static KeyCode FastKey = KeyCode.Equals;
        public static KeyCode PauseKey = KeyCode.R;
        public static KeyCode ResetKey = KeyCode.M;

        public static void DrawFrozenText()
        {
            GUI.Label(new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 150, 1000, 200), "<b><color=white><size=100>Frozen</size></color></b>");
        }
        
        public static void DrawWatermark()
        {
            string addition;
            if(updated)
            {
                addition = "updated";
            }
            else
            {
                addition = "out of date";
            }
            GUI.Label(new Rect((Screen.width / 2) - 150, Screen.height - 60, 1000, 200), "<b><color=white><size=30>UnTAS v" + version + " (" + addition + ")</size></color></b>");
        }

        public override void OnInitializeMelon()
        {
            WebClient wb = new WebClient();
            string latest = wb.DownloadString("https://raw.githubusercontent.com/EnderEye/UnTAS/master/version");
            if(latest == version + "\n")
            {
                updated = true;
            }
            else
            {
                updated = false;
            }
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

            if(Input.GetKeyDown(ResetKey))
            {
                baseTimeScale = 1f;
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
