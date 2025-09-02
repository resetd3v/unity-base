#region

using System;
using BepInEx.Logging;
using UnityEngine.SceneManagement;

#endregion

namespace plugin
{
    public class Utils
    {
        public static ManualLogSource Log = null!;
        
        public string GetScene() => SceneManager.GetActiveScene().name;
        public static readonly string userpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        // public static readonly string atmopath = userpath + "\\atmo\\";
        // public static readonly string dumppath = atmopath + "dump\\";
    }
}