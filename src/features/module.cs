#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace plugin.features
{
    // extending monobehaviour is prob bad and causing MASSIVE mem usage but is used for inject
    // can probably move DontDestroyOnLoad to OnStart in plugin.cs by finding Inject module and Inject.atmoGO 
    public class Module : MonoBehaviour
    {
        public string moduleName { get; }
        public string category { get; }
        public string description { get; }
        public KeyCode keycode { get; }
        public bool moduleEnabled;

        public Module(string name, string category, string description, KeyCode keycode = KeyCode.None, bool enabled = false) {
            this.moduleName = name;
            this.category = category;
            this.description = description;   
            this.keycode = keycode;
            this.moduleEnabled = enabled;
        }
        public Module(string name, string category, string description, bool enabled) : this(name, category, description, KeyCode.None, enabled) { }

        // runs on client start NOT onenable
        public virtual void OnStart() {}
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
        public virtual void OnRender() { }
        public virtual void OnSceneChange(Scene scene, LoadSceneMode mode) {}

        public void Toggle()
        {
            SetEnabled(!moduleEnabled);
        }

        public void SetEnabled(bool state)
        {
            moduleEnabled = state;
            if (moduleEnabled) OnEnable();
            else OnDisable();
        }
    }
}
