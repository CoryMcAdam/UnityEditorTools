using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMDev.EditorTools.Editor
{
    public static class EditorPlayFromScene
    {
        //Private
        private static SceneData _selectedScene;
        private static SceneData _previousScene;
        private static bool _startedFromDifferentScene;

        //Consts
        public const string PLAY_FROM_SCENE_PREF = "PlayFromScene.";
        public const string PLAY_FROM_SCENE_SCENE_PREF = PLAY_FROM_SCENE_PREF + "SavedScene";
        public const string PLAY_FROM_SCENE_ACTIVE_PREF = PLAY_FROM_SCENE_PREF + "StartedFromDifferentScene";
        public const string PLAY_FROM_SCENE_PREV_SCENE_PREF = PLAY_FROM_SCENE_PREF + "PreviousScene";

        //Properties
        public static string SelectedSceneName { get { return _selectedScene != null ? _selectedScene.Name : "None"; } }

        //Events
        public static event Action SelectedSceneUpdatedEvent;

        static EditorPlayFromScene()
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            LoadFromEditorPrefs();
        }

        public static void UpdateScene()
        {
            LoadFromEditorPrefs();
        }

        public static void ToggleSelectedScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
                return;

            if (_selectedScene?.Path == currentScene.path)
                SelectScene(null);
            else
                SelectScene(currentScene);
        }

        private static void SelectScene(Scene? scene)
        {
            _selectedScene = scene != null ? new SceneData(scene.Value) : null;

            SelectedSceneUpdatedEvent?.Invoke();
            SaveToEditorPrefs();
        }

        private static void SaveToEditorPrefs()
        {
            Logging.Log("[Play From Scene] Saving to editor prefs");
            Prefs.SetStringPref(PLAY_FROM_SCENE_SCENE_PREF, _selectedScene != null ? _selectedScene.Path : string.Empty);
        }

        private static void LoadFromEditorPrefs()
        {
            Logging.Log("[Play From Scene] Loading from editor prefs");

            string selectedScenePath = Prefs.GetStringPref(PLAY_FROM_SCENE_SCENE_PREF, "");
            _selectedScene = string.IsNullOrWhiteSpace(selectedScenePath) ? null : new SceneData(selectedScenePath);


            _previousScene = new SceneData(Prefs.GetStringPref(PLAY_FROM_SCENE_PREV_SCENE_PREF, ""));
            _startedFromDifferentScene = Prefs.GetBoolPref(PLAY_FROM_SCENE_ACTIVE_PREF, false);

            if (!string.IsNullOrWhiteSpace(selectedScenePath))
            {
                if (ClearMissingScene())
                    SaveToEditorPrefs();
            }
        }

        public static void ChangeSceneAndEnterPlaymode()
        {
            if (_selectedScene == null)
                return;

            if (!File.Exists(_selectedScene.Path))
                return;

            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.path != _selectedScene.Path)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                Prefs.SetBoolPref(PLAY_FROM_SCENE_ACTIVE_PREF, true);
                Prefs.SetStringPref(PLAY_FROM_SCENE_PREV_SCENE_PREF, currentScene.path);

                EditorSceneManager.OpenScene(_selectedScene.Path);
            }

            EditorApplication.EnterPlaymode();
        }

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            if (!_startedFromDifferentScene)
                return;

            if (stateChange == PlayModeStateChange.EnteredEditMode)
            {
                EditorSceneManager.OpenScene(_previousScene.Path);
                Prefs.SetBoolPref(PLAY_FROM_SCENE_ACTIVE_PREF, false);
            }
        }

        private static bool ClearMissingScene()
        {
            if (!Validation.ValidateScene(_selectedScene))
            {
                Logging.LogWarning($"[Play From Scene] Saved scene not found, removing from play from scene.");
                _selectedScene = null;

                return true;
            }

            return false;
        }
    }
}