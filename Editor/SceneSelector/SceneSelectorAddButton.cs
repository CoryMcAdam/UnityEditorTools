using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CMDev.EditorTools.Editor
{
    [EditorToolbarElement(ButtonID, typeof(SceneView))]
    public class SceneSelectorAddButton : EditorToolbarButton
    {
        public const string ButtonID = SceneSelectorToolbarOverlay.OverlayID + "/scene-selector-button-add";

        public SceneSelectorAddButton()
        {
            tooltip = "Add current Scene";
            icon = EditorGUIUtility.IconContent("d_toolbar plus").image as Texture2D;

            clicked += SceneSelectorAddButton_Clicked;

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            UpdateButtonState();
        }

        private void SceneSelectorAddButton_Clicked()
        {
            EditorSceneSelector.AddCurrentScene();
        }

        private void OnAttachToPanel(AttachToPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened += EditorSceneManager_SceneOpened;

            EditorSceneSelector.SavedScenesUpdatedEvent += EditorSceneSelector_SavedScenesUpdated;

            UpdateButtonState();
        }



        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;

            EditorSceneSelector.SavedScenesUpdatedEvent -= EditorSceneSelector_SavedScenesUpdated;

            UpdateButtonState();
        }

        private void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            UpdateButtonState();
        }

        private void EditorApplication_ProjectChanged()
        {
            UpdateButtonState();
        }

        private void EditorSceneManager_SceneOpened(Scene scene, OpenSceneMode sceneMode)
        {
            UpdateButtonState();
        }

        private void EditorSceneSelector_SavedScenesUpdated()
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (EditorApplication.isPlaying)
            {
                SetEnabled(false);
                return;
            }

            {
                Scene currentScene = SceneManager.GetActiveScene();

                for (int i = 0; i < EditorSceneSelector.SavedScenes.Count; i++)
                {
                    if (EditorSceneSelector.SavedScenes[i].Path == currentScene.path)
                    {
                        SetEnabled(false);
                        return;
                    }
                }
            }

            SetEnabled(true);
        }
    }
}
