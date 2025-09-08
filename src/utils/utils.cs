#region

using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace plugin;
public class Utils
{
    public static ManualLogSource Log = null!;
    
    public string GetScene() => SceneManager.GetActiveScene().name;
    public static readonly string userpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    public static readonly string atmopath = userpath + "\\atmo\\";
    // public static readonly string dumppath = atmopath + "dump\\";
    
    public static List<GameObject> GetAllObjects()
    {
        return new List<GameObject>((Resources.FindObjectsOfTypeAll(typeof(GameObject))as GameObject[])
        .Where((go) => (
            go.hideFlags != HideFlags.NotEditable &&
            go.hideFlags != HideFlags.HideAndDontSave &&
            go.transform.root.gameObject.activeInHierarchy
        )));
        
        List<GameObject> objs = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;

            if (go.transform.root.gameObject.activeInHierarchy) continue;

            objs.Add(go);
        }

        return objs;
    }

}