using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CMDev.EditorTools.Editor
{
    [EditorToolbarElement(ButtonID, typeof(SceneView))]
    public class PlayFromSceneSelectSceneButton : EditorToolbarButton
    {
        public const string ButtonID = PlayFromSceneToolbarOverlay.OverlayID + "/play-from-scene-select-scene-button";

        public PlayFromSceneSelectSceneButton()
        {
            tooltip = "Select current Scene";
            icon = EditorGUIUtility.IconContent("d_Favorite").image as Texture2D;

            text = EditorPlayFromScene.SelectedSceneName + " ";

            clicked += PlayFromSceneSelectSceneButton_Clicked;

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            UpdateButtonState();
        }

        private void PlayFromSceneSelectSceneButton_Clicked()
        {
            EditorPlayFromScene.ToggleSelectedScene();
        }

        private void OnAttachToPanel(AttachToPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened += EditorSceneManager_SceneOpened;

            EditorPlayFromScene.SelectedSceneUpdatedEvent += EditorPlayFromScene_SelectedSceneUpdatedEvent;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;

            EditorPlayFromScene.SelectedSceneUpdatedEvent -= EditorPlayFromScene_SelectedSceneUpdatedEvent;
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

        private void EditorPlayFromScene_SelectedSceneUpdatedEvent()
        {

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            text = EditorPlayFromScene.SelectedSceneName + " ";

            SetEnabled(!EditorApplication.isPlaying);
        }
    }
}