using UnityEngine;

namespace Exposed.Utils
{
    public static class ExposedLogger
    {
        public static void Info(object message, Object context = null)
        {
            if (ExposedScriptConfigurationManagerAsset.Instance.DebugLogging)
            {
                Debug.Log(message, context);
            }
        }

        public static void Warning(object message, Object context = null)
        {
            if (ExposedScriptConfigurationManagerAsset.Instance.DebugLogging)
            {
                Debug.LogWarning(message, context);
            }
        }

        public static void Error(object message, Object context = null)
        {
            if (ExposedScriptConfigurationManagerAsset.Instance.DebugLogging)
            {
                Debug.LogError(message, context);
            }
        }
         
    }
}