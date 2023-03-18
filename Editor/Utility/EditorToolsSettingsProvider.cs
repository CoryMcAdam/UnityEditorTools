using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    public class EditorToolsSettingsProvider : SettingsProvider
    {
        public EditorToolsSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }


        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("CM Editor Tools", SettingsScope.User)
            {
                label = "CM Editor Tools",

                guiHandler = (searchContext) =>
                {
                    Settings.DebuggingEnabled = EditorGUILayout.Toggle("Enable Debug", Settings.DebuggingEnabled);
                    Settings.HierarchyPrefixEnabled = EditorGUILayout.Toggle("Hierarchy Header Enabled", Settings.HierarchyPrefixEnabled);
                    Settings.HierarchyPrefix = EditorGUILayout.TextField("Hierarchy Header Prefix", Settings.HierarchyPrefix);
                },

                keywords = new HashSet<string>(new[] {"Debug", "Scene", "Selector"})
            };

            return provider;
        }
    }
}
