using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Toolbars;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

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
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;
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

        }

        private void EditorSceneManager_SceneOpened(Scene scene, OpenSceneMode sceneMode)
        {

        }
    }
}