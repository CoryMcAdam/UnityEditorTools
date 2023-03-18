using UnityEditor;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    [InitializeOnLoad]
    public static class HierarchyWindowGroupHeader
    {
        static HierarchyWindowGroupHeader()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (!Settings.HierarchyPrefixEnabled)
                return;

            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (gameObject == null)
                return;

            if (!gameObject.CompareTag("EditorOnly"))
                return;

            if (gameObject.name.StartsWith(Settings.HierarchyPrefix, System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace(Settings.HierarchyPrefix, "").ToUpperInvariant());
            }
        }
    }
}