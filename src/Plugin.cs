#region

using System.Net.WebSockets;
using System.Threading;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using plugin.features.impl;
using HarmonyLib;
using plugin.handlers;
using UnityEngine;
using UnityEngine.Playables;

#endregion

namespace plugin;

// uncomment for il2cpp
//[BepInPlugin(GUID, MODNAME, VERSION)]
/*public class Plugin : BaseUnityPlugin
{
    public const string
        MODNAME = "plugin",
        AUTHOR = "reset",
        GUID = "wtf." + AUTHOR + "." + MODNAME,
        VERSION = "1.0.0";
    public void Awake()
    {
        Logger.LogMessage("meow");
        Utils.Log = Logger;
        Meow.Log = Logger;
        Meow.Initialize(this);
    }
}*/

// \/ comment out for il2cpp \/
[BepInPlugin(GUID, MODNAME, VERSION)]
public class Meow : BaseUnityPlugin //MonoBehaviour <- switch for il2cpp
{
    public const string
        MODNAME = "strapped",
        AUTHOR = "reset",
        GUID = "wtf." + AUTHOR + "." + MODNAME,
        VERSION = "1.0.0";
    
    public static ManualLogSource Log = null!;
    public static GameObject pluginGO;
    public static Meow pluginComp;

    public Config config = new Config();
    public Utils utils = new Utils();

    public Communication com;

    public static Harmony Harmony = new(GUID);

    public bool toggled = true;
    public bool lastToggle = true;
    public bool lastDumpToggle = true;

    // uncomment for il2cpp
    /*public static void Initialize(Plugin plugin)
    {
        //pluginComp = plugin.AddComponent<Meow>();
        pluginComp.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(pluginComp.gameObject);
        pluginGO = pluginComp.gameObject;
    }*/

    // before anything is init
    private void Awake()
    {
        Logger.LogMessage("✦ -- MOD LOADED WOW!!!!!!!!!!!!!! -- ✦");
        //harmony shit
        //var harmony = new Harmony(GUID);
        
        Utils.Log = Logger;
        Log = Logger;
        
        //pluginComp = this.gameObject.AddComponent<Meow>();
        hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(gameObject);
        pluginGO = gameObject;
    }
    
    private void OnDestroy()
    {
        //com?.Close();
    }

    // called after objects are init
    private void Start()
    {
        Logger.LogMessage("moment meow");
        Harmony.PatchAll();
        
        //com = pluginGO.AddComponent<Communication>();
        //com = new Communication();
        //com.Init(Config.wsUrl);
        
        // retarded asf
        /*bool comInit = false;
        int timeout = 5000;
        int elapsed = 0;
        int checkInterval = 1;

        while (com.ws.State != WebSocketState.Open && elapsed < timeout)
        {
            if ((timeout - elapsed) % 1000 == 0) Utils.Log.LogInfo($"[comm] waited for {elapsed}ms (remaining: {timeout - elapsed}ms)");
            comInit = com.ws.State == WebSocketState.Open;
            if (comInit) break;
            
            Thread.Sleep(checkInterval);
            elapsed += checkInterval;
        }
        Utils.Log.LogInfo($"[comm] found comm in {elapsed}ms");
        
        comInit = com.ws.State == WebSocketState.Open;
        if (!comInit)
        {
            Utils.Log.LogError($"com failed {com.ws.State}");
            if (!Config.debug)
            {
                Utils.Log.LogError($"quitting! com failed");
                this.Destroy();
                return;
            }
        }*/
        
        ModuleManager.Init();
        ModuleManager.OnStart();
        //foreach(Inject module in ModuleManager.modules.Where(module => module.name == "Inject")) DontDestroyOnLoad(module.atmoGO);
    }

    // fire foreach event system (i really should hook certain events for performance to not enum thru gameobjects)
    private void Update()
    {
        UpdateInput();
        ModuleManager.OnUpdate();
    }

    private void OnGUI()
    {
        ModuleManager.OnRender();
    }

    private void UpdateInput()
    {
        ModuleManager.OnInput();
    }
}