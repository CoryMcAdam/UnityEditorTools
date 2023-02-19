using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMDev.EditorTools.Editor
{
    public static class EditorSceneSelector
    {
        private static List<SceneData> _savedScenes = new List<SceneData>();
        public static ReadOnlyCollection<SceneData> SavedScenes { get { return _savedScenes.AsReadOnly(); } }

        public const string SCENE_SELECTOR_PREF = "EditorSceneSelector.SavedScenes";

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
            string serializedScenes = string.Join(";", _savedScenes.Where(s => !string.IsNullOrEmpty(s.Path)).Select(s => s.Path));
            Prefs.SetStringPref(SCENE_SELECTOR_PREF, serializedScenes);
        }

        private static void LoadFromEditorPrefs()
        {
            string serializedScenes = Prefs.GetStringPref(SCENE_SELECTOR_PREF);

            _savedScenes = serializedScenes.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(s => new SceneData(s)).ToList();
        }
    }
}