using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CMDev.EditorTools.Editor
{
    [EditorToolbarElement(DropdownID, typeof(SceneView))]
    public class SceneSelectorDropdown : EditorToolbarDropdown
    {
        public const string DropdownID = SceneSelectorToolbarOverlay.OverlayID + "/scene-selector-dropdown";

        public SceneSelectorDropdown()
        {
            text = SceneManager.GetActiveScene().name;
            tooltip = "Select Scene";
            icon = EditorGUIUtility.IconContent("d_SceneAsset Icon").image as Texture2D;

            clicked += SceneSelectorDropdown_Clicked;

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void SceneSelectorDropdown_Clicked()
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < EditorSceneSelector.SavedScenes.Count; i++)
            {
                string scenePath = EditorSceneSelector.SavedScenes[i].Path;
                string sceneName = EditorSceneSelector.SavedScenes[i].Name;

                menu.AddItem(new GUIContent(sceneName), text == sceneName, () => OnDropdownItemSelected(sceneName, scenePath));
            }

            menu.DropDown(worldBound);
        }

        private void OnDropdownItemSelected(string sceneName, string scenePath)
        {
            text = sceneName;
            EditorSceneSelector.OpenScene(scenePath);
        }

        private void OnAttachToPanel(AttachToPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened += EditorSceneManager_SceneOpened;

            UpdateButtonState();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;

            UpdateButtonState();
        }

        private void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                    {
                        SetEnabled(true);
                    }
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    {
                        SetEnabled(false);
                    }
                    break;
            }
        }

        private void EditorApplication_ProjectChanged()
        {
            text = SceneManager.GetActiveScene().name;
        }

        private void EditorSceneManager_SceneOpened(Scene scene, OpenSceneMode sceneMode)
        {
            text = scene.name;
        }

        private void UpdateButtonState()
        {
            SetEnabled(!EditorApplication.isPlaying);
        }
    }
}
