using UnityEngine;

namespace Exposed
{
    public class ExposedScriptConfigurationManagerAsset : ScriptableObject
    {
        private static ExposedScriptConfigurationManagerAsset _instance;
        public static ExposedScriptConfigurationManagerAsset Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ExposedScriptConfigurationManagerAsset>("ExposedConfigurationManager");
                    if (_instance == null)
                    {
                        Debug.LogError("Exposed configuration manager wasn't found.");
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private bool _editorPreInitializationEnabled;
        public bool EditorPreInitializationEnabled { get { return _editorPreInitializationEnabled; } set { _editorPreInitializationEnabled = value; } }

        [SerializeField] private bool _buildPreInitializationEnabled = true;
        public bool BuildPreInitializationEnabled { get { return _buildPreInitializationEnabled; } set { _buildPreInitializationEnabled = value; } }

        [SerializeField] private bool _debugLogging;
        public bool DebugLogging { get { return _debugLogging; } set { _debugLogging = value; } }

        public bool PreInitializationEnabled
        {
            get
            {
#if !UNITY_EDITOR
                return _buildPreInitializationEnabled;
#else
                return _editorPreInitializationEnabled;
#endif
            }
        }
    }
}