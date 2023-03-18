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
        //PRIVATE FIELDS
        private static List<SceneData> _savedScenes = new List<SceneData>();

        //CONSTS
        public const string DEFAULT_PREF = "EditorSceneSelector.";
        public const string SCENE_SELECTOR_PREF = DEFAULT_PREF + "SavedScenes";

        //PROPERTIES
        public static ReadOnlyCollection<SceneData> SavedScenes { get { return _savedScenes.AsReadOnly(); } }
        
        //EVENTS
        public static event Action SavedScenesUpdatedEvent;

        /// <summary>
        /// Opens a scene through the EditorSceneManager. Prompts user to save changes before loading.
        /// </summary>
        /// <param name="scenePath">The scenes path.</param>
        public static void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        /// <summary>
        /// Loads the saved scenes list from editor prefs.
        /// </summary>
        public static void LoadScenesList()
        {
            LoadFromEditorPrefs();
        }

        /// <summary>
        /// Adds the current scene from the scene selector.
        /// </summary>
        public static void AddCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (AddSceneToList(currentScene))
            {
                SavedScenesChanged();
                SaveToEditorPrefs();
            }
        }

        /// <summary>
        /// Adds a scene to the saved scenes list.
        /// </summary>
        /// <param name="scene">The scene to add.</param>
        /// <returns>True if scene was added to list.</returns>
        private static bool AddSceneToList(Scene scene)
        {
            //Null scene.
            if (scene == null)
                return false;

            //Scene path already in list.
            if (_savedScenes.Any(s => s.Path == scene.path))
                return false;

            SceneData sceneData = new SceneData(scene);
            _savedScenes.Add(sceneData);
            return true;
        }

        /// <summary>
        /// Removes the current scene from the scene selector.
        /// </summary>
        public static void RemoveCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (RemoveSceneFromList(currentScene))
            {
                SavedScenesChanged();
                SaveToEditorPrefs();
            }
        }

        /// <summary>
        /// Removes a scene from the saved scenes list.
        /// </summary>
        /// <param name="scene">The scene to remove.</param>
        /// <returns>True if scene was removed from list.</returns>
        private static bool RemoveSceneFromList(Scene scene)
        {
            //Null scene.
            if (scene == null)
                return false;

            //Scene path in list.
            for (int i = 0; i < _savedScenes.Count; i++)
            {
                if (_savedScenes[i].Path == scene.path)
                {
                    _savedScenes.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Saves the current saved scenes list to editor prefs.
        /// </summary>
        private static void SaveToEditorPrefs()
        {
            Logging.Log("[Scene Selector] Saving to editor prefs");
            string serializedScenes = string.Join(";", _savedScenes.Where(s => !string.IsNullOrEmpty(s.Path)).Select(s => s.Path));
            Prefs.SetStringPref(SCENE_SELECTOR_PREF, serializedScenes);
        }

        /// <summary>
        /// Loads the saved scenes list from editor prefs.
        /// </summary>
        private static void LoadFromEditorPrefs()
        {
            Logging.Log("[Scene Selector] Loading from editor prefs");
            string serializedScenes = Prefs.GetStringPref(SCENE_SELECTOR_PREF);

            _savedScenes = serializedScenes.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(s => new SceneData(s)).ToList();

            //Check for missing scene paths and save if they are found.
            if (ClearMissingScenePaths())
                SaveToEditorPrefs();
        }

        private static void SavedScenesChanged()
        {
            ClearMissingScenePaths();
            SavedScenesUpdatedEvent?.Invoke();
        }

        /// <summary>
        /// Checks all saved scenes for missing paths.
        /// </summary>
        /// <returns>True if missing scene paths are found.</returns>
        private static bool ClearMissingScenePaths()
        {
            Logging.Log("[Scene Selector] Checking for missing scene paths.");

            if (_savedScenes.Count <= 0)
                return false;

            bool _isDirty = false;

            for (int i = _savedScenes.Count - 1; i >= 0; i--)
            {
                if (!Validation.ValidateScene(_savedScenes[i]))
                {
                    Logging.LogWarning($"[Scene Selector] Saved scene not found, removing from scene selector.");
                    _savedScenes.RemoveAt(i);
                    _isDirty = true;
                }
            }

            return _isDirty;
        }
    }
}