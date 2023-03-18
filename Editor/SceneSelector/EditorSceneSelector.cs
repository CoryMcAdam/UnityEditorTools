using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMDev.EditorTools.Editor
{
    public static class EditorSceneSelector
    {
        //Private
        private static List<SceneData> _savedScenes = new List<SceneData>();
        
        //Consts
        public const string SCENE_SELECTOR_PREF = "EditorSceneSelector.SavedScenes";

        //Properties
        public static ReadOnlyCollection<SceneData> SavedScenes { get { return _savedScenes.AsReadOnly(); } }
        
        //Events
        public static event Action SavedScenesUpdatedEvent;

        public static void OpenScene(string scene)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
        }

        public static void UpdateScenes()
        {
            LoadFromEditorPrefs();
        }

        public static void AddCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
                return;

            if (_savedScenes.Any(s => s.Path == currentScene.path))
                return;

            SceneData sceneData = new SceneData(currentScene);
            _savedScenes.Add(sceneData);

            SavedScenesUpdatedEvent?.Invoke();
            SaveToEditorPrefs();
        }

        public static void RemoveCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
                return;

            if (!_savedScenes.Any(s => s.Path == currentScene.path))
                return;

            for (int i = 0; i < _savedScenes.Count; i++)
            {
                if (_savedScenes[i].Path == currentScene.path)
                {
                    _savedScenes.RemoveAt(i);
                    break;
                }
            }

            SavedScenesUpdatedEvent?.Invoke();
            SaveToEditorPrefs();
        }

        private static void SaveToEditorPrefs()
        {
            ClearMissingScenePaths();

            string serializedScenes = string.Join(";", _savedScenes.Where(s => !string.IsNullOrEmpty(s.Path)).Select(s => s.Path));
            Prefs.SetStringPref(SCENE_SELECTOR_PREF, serializedScenes);
        }

        private static bool ClearMissingScenePaths()
        {
            if (_savedScenes.Count <= 0)
                return false;

            bool _isDirty = false;

            for (int i = _savedScenes.Count - 1; i >= 0; i--)
            {
                if (!File.Exists(_savedScenes[i].Path))
                {
                    Debug.LogWarning($"[Scene Selector] Saved scene {_savedScenes[i].Name} not found at path, removing from list.");
                    _savedScenes.RemoveAt(i);
                    _isDirty = true;
                }
            }

            return _isDirty;
        }

        private static void LoadFromEditorPrefs()
        {
            string serializedScenes = Prefs.GetStringPref(SCENE_SELECTOR_PREF);

            if (string.IsNullOrEmpty(serializedScenes))
                return;

            _savedScenes = serializedScenes.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(s => new SceneData(s)).ToList();

            if (ClearMissingScenePaths())
                SaveToEditorPrefs();
        }
    }
}