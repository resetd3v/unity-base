using System.Collections.Generic;
using System.Reflection;
using ComputerysModdingUtilities;
using HarmonyLib;
using UnityEngine;

namespace plugin.features.impl.player
{
    public class Restrictions : Module
    {
        public Restrictions() : base("Restrictions", "Player", "Removes \"Restrictions\" the player has") {}

        // prob better way
        private static bool hooksEnabled;
        
        public override void OnEnable()
        {
            ClientInstance instance = ClientInstance.Instance;
            if (!instance) return;
            hooksEnabled = true;
        }
        
        
        [HarmonyPatch(typeof(AssemblyScanner), "GetForeignAssemblies")]
        public class ModCheckPatch
        {
            public static void Postfix(ref List<Assembly> __result)
            {
                if (!hooksEnabled) return;
                
                __result =  new List<Assembly>();
                Utils.Log.LogInfo($"bypi: {__result}");
                //eturn true;
            }
        }

    }
}