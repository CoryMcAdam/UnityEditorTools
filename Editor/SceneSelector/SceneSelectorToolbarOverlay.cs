using UnityEditor;
using UnityEditor.Overlays;

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
            EditorSceneSelector.LoadScenesList();

            EditorApplication.projectChanged += EditorApplication_ProjectChanged;
        }

        public override void OnWillBeDestroyed()
        {
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
        }

        private void EditorApplication_ProjectChanged()
        {
            EditorSceneSelector.LoadScenesList();
        }
    }
}