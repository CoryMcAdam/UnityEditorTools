using System.Collections;
using System.Collections.Generic;
using UnityEditor.Toolbars;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Search;

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

            UpdateState();
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
        }



        private void OnDetachFromPanel(DetachFromPanelEvent evnt)
        {
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.projectChanged -= EditorApplication_ProjectChanged;
            EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;

            EditorSceneSelector.SavedScenesUpdatedEvent -= EditorSceneSelector_SavedScenesUpdated;
        }

        private void EditorSceneSelector_SavedScenesUpdated()
        {
            UpdateState();
        }

        private void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            UpdateState();
        }

        private void EditorApplication_ProjectChanged()
        {
            UpdateState();
        }

        private void EditorSceneManager_SceneOpened(Scene scene, OpenSceneMode sceneMode)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if(EditorApplication.isPlaying)
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
