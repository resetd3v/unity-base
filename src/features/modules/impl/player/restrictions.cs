using System;
using System.Collections.Generic;
using System.Reflection;
using ComputerysModdingUtilities;
using HarmonyLib;
using Steamworks;
using TMPro;
using UnityEngine;

namespace plugin.features.impl.player;
public class Restrictions : Module
{
    public Restrictions() : base("Restrictions", "Player", "Removes \"Restrictions\" the player has", true) {}

    // prob better way
    private static bool hooksEnabled;
    
    public override void OnEnable()
    {
        // ClientInstance instance = ClientInstance.Instance;
        // if (!instance) return;
        hooksEnabled = true;
        Utils.Log.LogInfo("restrictions enabled");
    }

    public override void OnDisable()
    {
        hooksEnabled = false;
        Utils.Log.LogInfo("restrictions disabled");
    }

    public override void OnUpdate()
    {
        if (FirstPersonController.instance)
        {
            MethodInfo dm = FirstPersonController.instance.GetType().GetMethod("AboubiPlayServer",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (dm == null) return;
            // dm.Invoke(FirstPersonController.instance, [5]);
        }
        // FirstPersonController.instance?.maxWallJumps = Int32.MaxValue;
    }
    
    
    [HarmonyPatch(typeof(TMP_Dropdown), "get_value")]
    public class PlayerPatch
    {
        public static void Postfix(ref int __result)
        {
            if (!hooksEnabled) return;

            __result = 10;
            Utils.Log.LogInfo($"players: {__result}");
            //eturn true;
        }
    }
    
    [HarmonyPatch(typeof(SteamMatchmaking), nameof(SteamMatchmaking.SetLobbyMemberLimit))]
    public class PlayerPatch2
    {
        public static void Prefix(ref int cMaxMembers)
        {
            if (!hooksEnabled) return;

            // cMaxMembers = -1;
            cMaxMembers = 10;
            Utils.Log.LogInfo($"players2: {cMaxMembers}");
            //eturn true;
        }
    }
    
    [HarmonyPatch(typeof(SteamLobby), "OnLobbyKicked")]
    public class AntiBanPacket
    {
        private static bool Prefix()
        {
            if (!hooksEnabled) return false;
            Utils.Log.LogInfo($"antiban method. $$$");
            return true;
            //eturn true;
        }
    }
    
    [HarmonyPatch(typeof(FirstPersonController), "HandleTimers")]
    public class FPSPatch
    {
        private static void Postfix(FirstPersonController __instance)
        {
            if (!hooksEnabled) return;
            
            FieldInfo fi = __instance.GetType().GetField("slideTimer",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                // fi.SetValue(__instance, float.MaxValue);
            }
            
            FieldInfo fi2 = __instance.GetType().GetField("slideResetTimer",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi2 != null)
            {
               // fi2.SetValue(__instance, float.MaxValue);
            }
        }
    }
    
    // [HarmonyPatch(typeof(FirstPersonController), "Update")]
    // public class WJPatch
    // {
    //     private static void Postfix(FirstPersonController __instance)
    //     {
    //         if (!hooksEnabled) return;
    //         __instance.maxWallJumps = int.MaxValue;
    //         __instance.wallJumpFactor = 2f;
    //         // Utils.Log.LogInfo($"wj method. $$$");
    //     }
    // }
    
    [HarmonyPatch(typeof(Weapon), nameof(Weapon.WeaponUpdate))]
    public class WJPatch
    {
        private static void Prefix(Weapon __instance)
        {
            if (!hooksEnabled) return;
            __instance.maxWallJumps = int.MaxValue;
            __instance.wallJumpFactor = 5f;
            // Utils.Log.LogInfo($"wj method. $$$");
        }
    }
    
    // [HarmonyPatch(typeof(FirstPersonController), "OnControllerColliderHit")]
    // public class WJPatch
    // {
    //     private static void Prefix(FirstPersonController __instance)
    //     {
    //         if (!hooksEnabled) return;
    //         __instance.maxWallJumps = int.MaxValue;
    //         // Utils.Log.LogInfo($"wj method. $$$");
    //     }
    // }
    
    // [HarmonyPatch(typeof(Weapon), "get_maxWallJumps")]
    // public class WJPatch
    // {
    //     public static void Prefix(ref int __result)
    //     {
    //         if (!hooksEnabled) return;
    //         __result = int.MaxValue;
    //         // Utils.Log.LogInfo($"wj method. $$$");
    //     }
    // }
}