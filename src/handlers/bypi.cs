using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ComputerysModdingUtilities;
using FishNet.Connection;
using FishNet.Object;
using HarmonyLib;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace plugin.features.impl;
public class Bypi : Module
{
    public Bypi() : base("Bypi", "AutoStart", "", true) {}

    #region
    public static GameObject maow = new GameObject("bruh");
    #endregion
    
    // public override void OnSceneChange(Scene scene, LoadSceneMode mode) => SceneMotor.Instance?.testMap = true;
    // public override void OnSceneChange(Scene scene, LoadSceneMode mode)
    // {
    //     Task.Delay(1000);
    //     GameObject ui = GameObject.FindGameObjectWithTag("PauseManager");
    //     GameObject minimal = ui.transform.Find("---MINIMAL UI---")?.gameObject;
    //     // GameObject minimal = GameObject.Find("---MINIMAL UI---");
    //     if (minimal) minimal.SetActive(true);
    // }

    public override void OnStart()
    {
        #region
        DontDestroyOnLoad(maow);
        // maow.AddComponent<PlayerTracker>();
        #endregion

        // SteamLobby.ownDlc0 = true;
        // SteamLobby.ownDlc1 = true;

        // var fi = AccessTools.Field(typeof(Application), "isEditor");
        // if (fi != null)
        // {
        //     fi.SetValue(null, true);
        // }
        // Task.Run(async () =>
        // {
        //     await Task.Delay(1000);
        //     // FieldInfo fi = AccessTools.Field(typeof(Application), "isEditor");
        //     FieldInfo fi = typeof(Application).GetField("isEditor",
        //         BindingFlags.NonPublic | BindingFlags.Static);
        //     if (fi != null)
        //     {
        //         fi.SetValue(null, true);
        //     }
        // });
    }

    // careful this is now spamming console by being called every tick for some reason 
    [HarmonyPatch(typeof(Application), "get_isEditor")]
    class Patch
    {
        static void Postfix(ref bool __result) => __result = true;
    }
    
    // bypi "ac"
    [HarmonyPatch(typeof(AssemblyScanner), "GetForeignAssemblies")]
    public class ModCheckPatch
    {
        public static void Postfix(ref List<Assembly> __result)
        {
            __result =  new List<Assembly>();
            Utils.Log.LogInfo($"bypi: {__result.Count}");
            //eturn true;
        }
    }
    
    [HarmonyPatch(typeof(NetworkBehaviour), nameof(NetworkBehaviour.OwnerMatches))]
    class NoPatch
    {
        static void Postfix(ref bool __result, NetworkConnection connection)
        {
            if (!__result) Utils.Log.LogInfo($"nice: {__result} | {connection.GetAddress()}");   
        }
    }
    
    // [HarmonyPatch(typeof(SteamLobby), "get_ownDlc0")]
    // class SryPatch
    // {
    //     static void Postfix(ref bool __result) => __result = true;
    // }
    // [HarmonyPatch(typeof(SteamLobby), "get_ownDlc1")]
    // class SryPatch2
    // {
    //     static void Postfix(ref bool __result) => __result = true;
    // }
    // check in heathensteamworks api using base64 to log DLC cheating dtc
    [HarmonyPatch(typeof(CosmeticInstance), "Start")]
    class SryPatch3
    {
        static void Postfix()
        {
            SteamLobby.ownDlc0 = true;
            SteamLobby.ownDlc1 = true;
        }
    }
    [HarmonyPatch(typeof(SteamApps), nameof(SteamApps.BIsDlcInstalled))]
    class SryPatch4
    {
        static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }


    //[HarmonyPatch(typeof(AirshipSimulationManager), "PerformResimulation")]
    //public class ResimPatch
    //{
    //    public static bool Prefix(AirshipSimulationManager __instance, int baseTick)
    //    {
    //        Utils.Log.LogInfo($"bypi: {baseTick}");
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(AirshipNetworkedObject), "LagCompensationCheck")]
    //public class ResimPatch2
    //{
    //    public static bool Prefix(AirshipNetworkedObject __instance, int clientId, int tick, double time, double latency, double bufferTime)
    //    {
    //        Utils.Log.LogInfo($"bypi2: {latency}");
    //        latency = 100d;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(AirshipSimulationManager), "ScheduleResimulation")]
    //public class ResimPatch2
    //{
    //    public static bool Prefix(AirshipSimulationManager __instance, PerformResimulationCallback callback)
    //    {
    //        Utils.Log.LogInfo($"bypi2: {callback}");
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(AirshipSimulationManager), "RequestLagCompensationCheck")]
    //public class ResimPatch3
    //{
    //    public static void Prefix(AirshipSimulationManager __instance, int baseTick)
    //    {
    //        Utils.Log.LogInfo($"bypi3: {baseTick}");
    //        return;
    //    }
    //}
}