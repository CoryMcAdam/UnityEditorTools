using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CMDev.EditorTools.Editor
{
    [EditorToolbarElement(ButtonID, typeof(SceneView))]
    public class SceneSelectorRemoveButton : EditorToolbarButton
    {
        public const string ButtonID = SceneSelectorToolbarOverlay.OverlayID + "/scene-selector-button-remove";

        public SceneSelectorRemoveButton()
        {
            tooltip = "Remove current Scene";
            icon = EditorGUIUtility.IconContent("d_toolbar minus").image as Texture2D;

            clicked += SceneSelectorRemoveButton_Clicked;

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            UpdateButtonState();
        }

        private void SceneSelectorRemoveButton_Clicked()
        {
            EditorSceneSelector.RemoveCurrentScene();
        }

        private void OnAttachToPanel(AttachToPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened += EditorSceneManager_SceneOpened;

            EditorSceneSelector.SavedScenesUpdatedEvent += EditorSceneSelector_SavedScenesUpdated;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;

            EditorSceneSelector.SavedScenesUpdatedEvent -= EditorSceneSelector_SavedScenesUpdated;
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
                        SetEnabled(true);
                        return;
                    }
                }
            }

            SetEnabled(false);
        }
    }
}