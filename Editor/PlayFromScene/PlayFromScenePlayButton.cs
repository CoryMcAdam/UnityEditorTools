using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace CMDev.EditorTools.Editor
{
    [EditorToolbarElement(ButtonID, typeof(SceneView))]
    public class PlayFromScenePlayButton : EditorToolbarButton
    {
        public const string ButtonID = PlayFromSceneToolbarOverlay.OverlayID + "/play-from-scene-play-button";

        public PlayFromScenePlayButton()
        {
            tooltip = "Play from selected scene";
            icon = EditorGUIUtility.IconContent("d_PlayButton").image as Texture2D;

            clicked += PlayFromScenePlayButton_Clicked;

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            UpdateButtonState();
        }

        private void PlayFromScenePlayButton_Clicked()
        {
            EditorPlayFromScene.ChangeSceneAndEnterPlaymode();
        }

        private void OnAttachToPanel(AttachToPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            EditorSceneSelector.SavedScenesUpdatedEvent += EditorSceneSelector_SavedScenesUpdated;

            UpdateButtonState();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorSceneSelector.SavedScenesUpdatedEvent -= EditorSceneSelector_SavedScenesUpdated;

            UpdateButtonState();
        }

        private void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            UpdateButtonState();
        }

        private void EditorSceneSelector_SavedScenesUpdated()
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            SetEnabled(!EditorApplication.isPlaying);
        }
    }
}
