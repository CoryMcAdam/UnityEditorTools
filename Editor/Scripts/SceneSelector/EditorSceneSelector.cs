using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMDev.EditorTools.Editor
{
    public static class EditorSceneSelector
    {
        public static readonly List<string> Scenes = new List<string>();
        public static readonly List<string> SavedScenes = new List<string>();

        public static void OpenScene(string scene)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
        }

        public static void UpdateScenes()
        {
            Scenes.Clear();
            SavedScenes.Clear();

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");

            foreach (var sceneGuid in sceneGuids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                Scenes.Add(scenePath);
            }
        }

        public static void AddCurrentScene()
        {
            string scenePath = SceneManager.GetActiveScene().path;

            if (!scenePath.Contains(scenePath))
            {
                Debug.Log("Added scene");
                SavedScenes.Add(scenePath);
            }
        }

        public static void RemoveCurrentScene()
        {
            string scenePath = SceneManager.GetActiveScene().path;

            if (scenePath.Contains(scenePath))
            {
                Debug.Log("Removed scene");
                SavedScenes.Remove(scenePath);
            }
        }
    }
}