#region

using System.IO;
using plugin;
using plugin.features;
using UnityEngine;
using MemoryStream = System.IO.MemoryStream;

#endregion

namespace bridge.features.impl
{
    public class Inject : Module
    {
        public Inject() : base("Inject", "AutoStart", "", true) { }

        private readonly string _debugFilepath = Utils.atmopath + "bruh.bundle";
        
        public GameObject atmoGO;

        public override void OnStart()
        {
            byte[] buf;

            if (Config.debug)
            {
                if (!File.Exists(_debugFilepath))
                {
                    Utils.Log.LogError($"failed to find atmo bundle file -> {_debugFilepath}");
                    return;
                }
                buf = File.ReadAllBytes(_debugFilepath);
                Utils.Log.LogInfo($"found atmo bundle file -> {_debugFilepath}");
                if (!InjectBundle(buf))
                {
                    Utils.Log.LogError($"failed to inject bundle -> {_debugFilepath}");
                }
                return;
            }
            
            // retrieve from communicaiton instead here
        }

        private bool InjectBundle(byte[] bundleBytes)
        {
            // read from websocket soon:tm:
            var stream = new MemoryStream(bundleBytes);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            if (!bundle)
            {
                Utils.Log.LogError($"failed to load atmo bundle -> {_debugFilepath}");
                return false;
            }

            //api.PushUnityObject(luaState, bundle);
            stream.Close();

            Utils.Log.LogInfo($"attempting go load -> {bundle.name}");
            var atmoBundleGO = bundle.LoadAsset<GameObject>("UI");
            if (!atmoBundleGO)
            {
                Utils.Log.LogError($"failed to load atmo go -> {bundle.name}");
                return false;
            }

            //foreach (GameObject go in atmoGO)
            {
                Utils.Log.LogInfo($"loaded atmo go -> {atmoBundleGO.name}");
                atmoGO = Instantiate(atmoBundleGO);
                Utils.Log.LogInfo($"init go success!!!1!!1 :3 {atmoGO.name}");
                DontDestroyOnLoad(atmoGO);
            }
            return true;
        }
    }
}
