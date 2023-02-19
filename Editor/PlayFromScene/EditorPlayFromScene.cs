using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMDev.EditorTools.Editor
{
    [InitializeOnLoad]
    public static class EditorPlayFromScene
    {
        private static SceneData _selectedScene;

        public static event Action SelectedSceneUpdatedEvent;

        public const string PLAY_FROM_SCENE_SCENE_PREF = "PlayFromScene.SavedScene";
        public const string PLAY_FROM_SCENE_ACTIVE_PREF = "PlayFromScene.Active";
        public const string PLAY_FROM_SCENE_PREV_SCENE_PREF = "PlayFromScene.PreviousScene";

        public static string SelectedSceneName { get { return _selectedScene != null ? _selectedScene.Name : "None"; } }

        static EditorPlayFromScene()
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
        }

        public static void UpdateScene()
        {
            LoadFromEditorPrefs();
        }

        public static void SelectScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
                return;

            if (_selectedScene?.Path == currentScene.path)
                DeselectCurrentScene();
            else
                SelectCurrentScene();
        }

        private static void SelectCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
                return;

            _selectedScene = new SceneData(currentScene);

            SelectedSceneUpdatedEvent?.Invoke();
            SaveToEditorPrefs();
        }

        private static void DeselectCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
                return;

            _selectedScene = null;

            SelectedSceneUpdatedEvent?.Invoke();
            SaveToEditorPrefs();
        }

        private static void SaveToEditorPrefs()
        {
            Prefs.SetStringPref(PLAY_FROM_SCENE_SCENE_PREF, _selectedScene != null ? _selectedScene.Path : string.Empty);
        }

        private static void LoadFromEditorPrefs()
        {
            _selectedScene = new SceneData(Prefs.GetStringPref(PLAY_FROM_SCENE_SCENE_PREF));
        }

        public static void ChangeSceneAndEnterPlaymode()
        {
            Debug.Log("Changing scene and entering playmode");
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.path != _selectedScene.Path)
            {
                Debug.Log("Played from different scene");

                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                Prefs.SetBoolPref(PLAY_FROM_SCENE_ACTIVE_PREF, true);
                Prefs.SetStringPref(PLAY_FROM_SCENE_PREV_SCENE_PREF, currentScene.path);

                EditorSceneManager.OpenScene(_selectedScene.Path);
            }

           

            EditorApplication.EnterPlaymode();

            Debug.Log("Changed scene and entered playmode");
        }

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            Debug.Log("Play mode changed");

            bool playedFromDifferentScene = Prefs.GetBoolPref(PLAY_FROM_SCENE_ACTIVE_PREF);

            if (!playedFromDifferentScene)
                return;

            Debug.Log("Played from different scene");

            if (stateChange == PlayModeStateChange.EnteredEditMode)
            {
                SceneData previousScene = new SceneData(Prefs.GetStringPref(PLAY_FROM_SCENE_PREV_SCENE_PREF));

                Debug.Log($"Opening {previousScene.Name}");
                EditorSceneManager.OpenScene(previousScene.Path);


                Prefs.SetBoolPref(PLAY_FROM_SCENE_ACTIVE_PREF, false);

            }

            
        }
    }
}