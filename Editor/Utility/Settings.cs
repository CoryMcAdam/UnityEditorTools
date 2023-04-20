using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMDev.EditorTools.Editor
{
    internal static class Settings
    {
        //CONSTS
        const string DEBUG_PREF = Prefs.SETTINGS_PREF + "DebugEnabled";
        const string HIERARCHY_PREFIX_PREF = Prefs.SETTINGS_PREF + "HierarchyPrefix";
        const string HIERARCHY_PREFIX_ENABLED_PREF = Prefs.SETTINGS_PREF + "HierarchyPrefixEnabled";
        const string HIERARCHY_DEFAULT_PREFIX = "---";

        //PRIVATE FIELDS
        private static bool? _debuggingEnabled;
        private static string _hierarchyPrefix;
        private static bool? _hierarchyPrefixEnabled;

        //PROPERTIES
        public static bool DebuggingEnabled
        {
            get
            {
                if(!_debuggingEnabled.HasValue)
                    _debuggingEnabled = Prefs.GetBoolPref(DEBUG_PREF, false);

                return _debuggingEnabled.Value;
            }
            set
            {
                _debuggingEnabled = value;
                Prefs.SetBoolPref(DEBUG_PREF, value);
            }
        }

        public static string HierarchyPrefix
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_hierarchyPrefix))
                    _hierarchyPrefix = Prefs.GetStringPref(HIERARCHY_PREFIX_PREF, HIERARCHY_DEFAULT_PREFIX);

                if (_hierarchyPrefix.Length < 1)
                {
                    _hierarchyPrefix = HIERARCHY_DEFAULT_PREFIX;
                    Logging.LogWarning("Prefix must be at least one character long.");
                }

                return _hierarchyPrefix;
            }
            set
            {
                _hierarchyPrefix = value;
                Prefs.SetStringPref(HIERARCHY_PREFIX_PREF, value);
            }
        }

        public static bool HierarchyPrefixEnabled
        {
            get
            {
                if (!_hierarchyPrefixEnabled.HasValue)
                    _hierarchyPrefixEnabled = Prefs.GetBoolPref(HIERARCHY_PREFIX_ENABLED_PREF, true);

                return _hierarchyPrefixEnabled.Value;
            }

            set
            {
                _hierarchyPrefixEnabled = value;
                Prefs.SetBoolPref(HIERARCHY_PREFIX_ENABLED_PREF, value);
            }
        }
    }
}