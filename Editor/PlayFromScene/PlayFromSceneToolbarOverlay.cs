using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    [Overlay(typeof(SceneView), OverlayID, "Play From Scene Overlay")]
    public class PlayFromSceneToolbarOverlay : ToolbarOverlay
    {
        public const string OverlayID = "play-from-scene-overlay";

        private PlayFromSceneToolbarOverlay() : base(
            PlayFromScenePlayButton.ButtonID,
            PlayFromSceneSelectSceneButton.ButtonID
            )
        { }

        public override void OnCreated()
        {
            EditorPlayFromScene.UpdateScene();

            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
        }

        public override void OnWillBeDestroyed()
        {
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
        }

        private void EditorApplication_ProjectChanged()
        {
            EditorPlayFromScene.UpdateScene();
        }
    }
}
