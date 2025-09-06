#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#endregion

namespace plugin.handlers;
public static class CommandManager
{
    public static List<Module> modules = new List<Module>();

    public static void Init()
    {
        RegisterCommands();
        Utils.Log.LogInfo($"registered {modules.Count} modules");
    }
    
    private static void RegisterCommands()
    {
        Type[] moduleTypes = FindCommands();
        // modules.AddRange(moduleTypes.Select(Activator.CreateInstance));
        modules.AddRange(moduleTypes.Select(type => (Module)Activator.CreateInstance(type)));
    }

    // notes to remember after smoke break
    // reflection to find module files | maybe copy and use harmonypatch annotiation? make module info annotation
    public static Type[] FindCommands()
    {
        Assembly thisAssembly = Assembly.GetExecutingAssembly();
        return thisAssembly.GetTypes().Where(type => type.BaseType == typeof(Module)).ToArray();
    }

}