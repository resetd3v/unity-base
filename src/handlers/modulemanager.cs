#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#endregion

namespace plugin.features.impl;
public static class ModuleManager
{
    public static List<Module> modules = new List<Module>();

    public static void Init()
    {
        RegisterModules();
        Utils.Log.LogInfo($"registered {modules.Count} modules");
    }
    
    private static void RegisterModules()
    {
        Type[] moduleTypes = FindModules();
        // modules.AddRange(moduleTypes.Select(Activator.CreateInstance));
        modules.AddRange(moduleTypes.Select(type => (Module)Activator.CreateInstance(type)));
    }

    // notes to remember after smoke break
    // reflection to find module files | maybe copy and use harmonypatch annotiation? make module info annotation
    public static Type[] FindModules()
    {
        Assembly thisAssembly = Assembly.GetExecutingAssembly();
        return thisAssembly.GetTypes().Where(type => type.BaseType == typeof(Module)).ToArray();
    }
    
    
    private static void UpdateCache()
    {
        //if (!Cache.selfPlayer) Cache.selfPlayer = Settings.Instance.localPlayer?.gameObject;
        Cache.selfPlayer = Settings.Instance.localPlayer?.gameObject;
    }

    // fire foreach event system (i really should hook certain events for performance to not enum thru gameobjects)
    public static void OnStart()
    {
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
        foreach (var module in modules) module.OnStart(); //.Where(module => module.moduleEnabled)
    }

    public static void OnUpdate()
    {
        UpdateCache();
        foreach (var module in modules.Where(module => module.moduleEnabled)) module.OnUpdate();
    }

    public static void OnRender()
    {
        foreach (var module in modules.Where(module => module.moduleEnabled)) module.OnRender();
    }

    public static void OnInput()
    {
        foreach (var module in modules.Where(module => module.keycode != KeyCode.None && Input.GetKeyUp(module.keycode))) module.Toggle();
    }

    // retarded ik
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var module in modules.Where(module => module.moduleEnabled)) module.SetEnabled(true);
        foreach (var module in modules.Where(module => module.moduleEnabled)) module.OnSceneChange(scene, mode);
    }

}