using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map;
using UnityEngine;

namespace kys
{
    public class Config
    {
        public static int delay = 2;
        public static List<string> blacklistedScenes = new List<string> { "loadingbasic", "splash", "intro", "title", "lobby" };
        //public Dictionary<string, Texture2D> texReplace = new Dictionary<string, Texture2D> { { "ScopeOverlay", Meow.scopeTex } };
        public Dictionary<string, Action<Texture2D>> texReplace = new Dictionary<string, Action<Texture2D>> { { "ScopeOverlay", texture => Meow.scopeTex = texture } };
        public string texReplacement = "clown_mask_icon";
        public Color colReplacement = new Color(1, 1, 1, 0.15f);
    }
}
