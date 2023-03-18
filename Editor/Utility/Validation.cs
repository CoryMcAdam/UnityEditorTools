using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    internal static class Validation
    {
        public static bool ValidateScene(SceneData sceneData)
        {
            if (!File.Exists(sceneData?.Path) || string.IsNullOrWhiteSpace(sceneData?.Name))
            {
                Logging.LogWarning($"Scene \"{sceneData?.Name}\" not found at path \"{sceneData?.Path}\"");
                return false;
            }

            return true;
        }
    }
}