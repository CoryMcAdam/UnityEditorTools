using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    internal static class Logging
    {
        public static void Log(string message)
        {
            if (Settings.DebuggingEnabled)
                Debug.Log(message);
        }

        public static void LogWarning(string message)
        {
            if (Settings.DebuggingEnabled)
                Debug.LogWarning(message);
        }

        public static void LogError(string message)
        {
            if (Settings.DebuggingEnabled)
                Debug.LogError(message);
        }
    }
}