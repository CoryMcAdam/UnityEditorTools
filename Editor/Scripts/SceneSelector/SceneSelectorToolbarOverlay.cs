using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    [Overlay(typeof(SceneView), OverlayID, "Scene Selector Overlay")]
    public class SceneSelectorToolbarOverlay : ToolbarOverlay
    {
        public const string OverlayID = "scene-selector-overlay";

        private SceneSelectorToolbarOverlay() : base(
            SceneSelectorDropdown.DropdownID,
            SceneSelectorAddButton.ButtonID,
            SceneSelectorRemoveButton.ButtonID
            )
        { }

        public override void OnCreated()
        {
            EditorSceneSelector.UpdateScenes();

            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
        }

        public override void OnWillBeDestroyed()
        {
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
        }

        private void EditorApplication_ProjectChanged()
        {
            EditorSceneSelector.UpdateScenes();
        }
    }
}