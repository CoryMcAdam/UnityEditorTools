using UnityEditor;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    internal static class Prefs
    {
        public const string SETTINGS_PREF = "Settings.";

        #region SetPrefs
        public static void SetStringPref(string prefName, string value)
        {
            EditorPrefs.SetString($"{Application.productName}.{prefName}", value);
        }

        public static void SetBoolPref(string prefName, bool value)
        {
            EditorPrefs.SetBool($"{Application.productName}.{prefName}", value);
        }

        public static void SetIntPref(string prefName, int value)
        {
            EditorPrefs.SetInt($"{Application.productName}.{prefName}", value);
        }
        #endregion

        #region GetPrefs
        public static string GetStringPref(string prefName)
        {
            return EditorPrefs.GetString($"{Application.productName}.{prefName}");
        }

        public static string GetStringPref(string prefName, string defaultValue)
        {
            return EditorPrefs.GetString($"{Application.productName}.{prefName}", defaultValue);
        }

        public static bool GetBoolPref(string prefName)
        {
            return EditorPrefs.GetBool($"{Application.productName}.{prefName}");
        }

        public static bool GetBoolPref(string prefName, bool defaultValue)
        {
            return EditorPrefs.GetBool($"{Application.productName}.{prefName}", defaultValue);
        }

        public static int GetIntPref(string prefName)
        {
            return EditorPrefs.GetInt($"{Application.productName}.{prefName}");
        }

        public static int GetIntPref(string prefName, int defaultValue)
        {
            return EditorPrefs.GetInt($"{Application.productName}.{prefName}", defaultValue);
        }
        #endregion
    }
}