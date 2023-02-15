using System;
using System.Collections;
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

        public const string SAVED_SCENES_PREF = "EditorSceneSelector.SavedScenes";

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

            Debug.Log("Added scene");

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
                    Debug.Log("Removed scene");
                    break;
                }
            }

            SavedScenesUpdatedEvent?.Invoke();
            SaveToEditorPrefs();
        }

        private static void SaveToEditorPrefs()
        {
            string serializedScenes = string.Join(";", _savedScenes.Where(s => !string.IsNullOrEmpty(s.Path)).Select(s => s.Path));
            SetPref(SAVED_SCENES_PREF, serializedScenes);
        }

        private static void SetPref(string prefName, string value)
        {
            EditorPrefs.SetString($"{Application.productName}.{prefName}", value);
        }

        private static void LoadFromEditorPrefs()
        {
            string serializedScenes = GetPref(SAVED_SCENES_PREF);

            _savedScenes = serializedScenes.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(s => new SceneData(s)).ToList();
        }

        private static string GetPref(string prefName)
        {
            return EditorPrefs.GetString($"{Application.productName}.{prefName}");
        }

        [Serializable]
        public class SceneData
        {
            public string Name;
            public string Path;

            public SceneData(Scene scene)
            {
                Name = scene.name;
                Path = scene.path;
            }

            public SceneData(string path)
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(path);
                Path = path;
            }
        }
    }
}