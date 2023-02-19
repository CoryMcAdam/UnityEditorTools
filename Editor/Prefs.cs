using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    internal static class Prefs
    {
        public static void SetStringPref(string prefName, string value)
        {
            EditorPrefs.SetString($"{Application.productName}.{prefName}", value);
        }

        public static void SetBoolPref(string prefName, bool value)
        {
            EditorPrefs.SetBool($"{Application.productName}.{prefName}", value);
        }

        public static string GetStringPref(string prefName)
        {
            return EditorPrefs.GetString($"{Application.productName}.{prefName}");
        }

        public static bool GetBoolPref(string prefName)
        {
            return EditorPrefs.GetBool($"{Application.productName}.{prefName}");
        }
    }
}