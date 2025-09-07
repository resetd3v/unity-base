using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

using static kys.Utils;
using static kys.Config;
using BepInEx.Unity.IL2CPP;
using static Il2CppSystem.Linq.Expressions.Interpreter.CastInstruction.CastInstructionNoT;
using static HarmonyLib.Code;
using UnityEngine.UI;

namespace kys
{

    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Plugin : BasePlugin
    {
        public const string
            MODNAME = "kys",
            AUTHOR = "reset",
            GUID = "net." + AUTHOR + "." + MODNAME,
            VERSION = "1.0.0";
        public override void Load()
        {
            Log.LogMessage("meow");
            Utils.Log = Log;
            Meow.Log = Log;
            Meow.Initialize(this);
        }
    }

    public class Meow : MonoBehaviour
    {
        public static ManualLogSource Log = null!;

        public Config config = new Config();
        public Utils utils = new Utils();

        public static Harmony Harmony = new(Plugin.GUID);

        private Aim aim = new Aim();
        private ESP esp = new ESP();

        public bool toggled = true;
        public bool lastToggle = true;

        public static RawImage scopeImage;
        public static Texture2D scopeTex;
        public static Texture2D replaceTex;

        public static void Initialize(Plugin plugin)
        {
            Meow addComponent = plugin.AddComponent<Meow>();
            addComponent.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(addComponent.gameObject);
        }


        private void Awake()
        {
            Log.LogMessage("\u001b[31mMOD LOADED WOW!!!!!!!!!!!!!!\u001b[0m");
            //harmony shit
            //var harmony = new Harmony(GUID);
        }

        private void Start()
        {
            Log.LogMessage("moment");
            Harmony.PatchAll();

            Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            foreach (Texture2D texture in textures)
            {
                if (texture.name == config.texReplacement) replaceTex = texture;
                if (texture.name == "ScopeOverlay") scopeTex = texture;

                if (texture == null || !config.texReplace.ContainsKey(texture.name)) continue;

                Log.LogInfo($"Found tex {texture} | {texture.name}");
                //config.texReplace[texture.name](texture);

                //Texture2D value;
                //if (config.texReplace.TryGetValue(texture.name, out value))
                //{
                //    value = texture;

                //    //ImageConversion.LoadImage(value, image);
                //}
            }

            ModScope(replaceTex, config.colReplacement);
        }

        private void Update()
        {
            try
            {
                //SceneManager.sceneLoaded += (_, _) => { loaded.Clear(); replace.Clear(); funniloaded = false; funniloaded2 = false; }; //Log.LogDebug("cleared"); };
                lastToggle = toggled;
                UpdateInput();
                if (lastToggle != toggled)
                {
                    Log.LogInfo($"toggled da MEOW {((toggled) ? "on" : "off")}");

                    if (toggled) ModScope(replaceTex, config.colReplacement);
                    else UnModScope(scopeTex, new Color(1, 1, 1, 1));
                }
                //if (!toggled || utils.IsSceneBlacklisted()) return;
                //if (loaded.Count != texReplace.Count) StartCoroutine(TryTextureReplace());
            }
            catch (Exception e)
            {
                Log.LogError(e.StackTrace);
            }

            aim.OnUpdate();
        }


        private void OnGUI()
        {
            aim.OnRender();
            esp.OnRender();
        }

        private void UpdateInput()
        {
            if (Input.GetKeyUp(KeyCode.DownArrow)) toggled = !toggled;
        }

        private void ModScope(Texture2D tex, Color col)
        {
            RawImage[] images = GameObject.FindObjectsOfType<RawImage>(true);
            foreach (RawImage image in images)
            {
                if (image == null || image.name != "Scope") continue;

                Texture old = image.texture;
                image.texture = tex;
                image.color = col;
                Log.LogInfo($"Replaced scope {image.name} = {image.texture.name} | {old.name}");
            }
        }

        private void UnModScope(Texture2D tex, Color col)
        {
            RawImage[] images = GameObject.FindObjectsOfType<RawImage>(true);
            foreach (RawImage image in images)
            {
                if (image == null || image.name != "Scope") continue;

                Texture old = image.texture;
                image.texture = tex;
                image.color = col;
                Log.LogInfo($"Replaced scope {image.name} = {image.texture.name} | {old.name}");
            }
        }
    }
}