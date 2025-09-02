using UnityEngine;

namespace plugin;

public static class Cache
{
    public static GameObject selfPlayer = Settings.Instance.localPlayer?.gameObject;
    public static GameObject aimTarg;
}