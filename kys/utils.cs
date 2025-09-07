using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine.SceneManagement;

using static kys.Config;

namespace kys
{
    public class Utils
    {
        public static ManualLogSource Log = null!;
        public string GetScene() => SceneManager.GetActiveScene().name;
    }
}