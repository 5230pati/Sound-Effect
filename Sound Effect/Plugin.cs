using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Media;
using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Linq;

namespace Sound_Effect
{
    public class SoundEffectPlugin : IPlugin
    {
        public string Name => "Sound Effect";
        public string Version => "2.3.2";

        public static string PluginName
        {
            get { return Instance.Name; }
        }

        static SoundEffectPlugin Instance;

        /////////////////////////////////////////////////////

        public static string GetExecutingDirectoryName()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).Directory.FullName;
        }

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Instance = this;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            // 0 - Init
            // 1 - HealthWarning
            // 2 - Menu
            // 3 - StandardLevelLoader
            // 4 - ???
            // 5 - ???
            // 6 - ???
            // 7 - ???
            // 8 - NiceEnvironment
            // 9 - DeafultEnvironment
            // 10- BigMirrorEnvironment
            // 11- TriangleEnvironment
            if (arg1.name == "NiceEnvironment" ||
                arg1.name == "DefaultEnvironment" ||
                arg1.name == "BigMirrorEnvironment" ||
                arg1.name == "TriangleEnvironment")
            {
                new GameObject("SoundEffect").AddComponent<SoundEffect>();
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "Menu")
            {
                SoundEffectUI.OnLoad();

                SoundEffect._dist = ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Distortion", true, false);
                SoundEffect._distLen = ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "DistortionLength", 0.15f, false);
                SoundEffect._miss = ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Miss", true, false);
                SoundEffect._missVol = ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "MissVolume", 0.75f, false);
                SoundEffect._bomb = ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Bomb", true, false);
                SoundEffect._bombVol = ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "BombVolume", 0.75f, false);
                SoundEffect._hit = ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Hit", true, false);
                SoundEffect._hitVol = ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "HitVolume", 0.75f, false);
                SoundEffect._bad = ModPrefs.GetBool(SoundEffectPlugin.PluginName, "BadHit", true, false);
                SoundEffect._badVol = ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "BadHitVolume", 0.75f, false);
                SoundEffect._fail = ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Fail", true, false);
                SoundEffect._failVol = ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "FailVolume", 0.75f, false);
            }
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
