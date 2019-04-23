using Exposed.Utils;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Exposed.Editor.Scene
{
    public class ExposedScenePostprocessor
    {
        [PostProcessScene]
        public static void OnPostprocessScene()
        {
            //it is fired also in playmode in editor after playmode is entered (not before)
            if (ExposedScriptConfigurationManagerAsset.Instance.BuildPreInitializationEnabled && !EditorApplication.isPlaying)
            {
                ExposedLogger.Info("Running EXPOSED scene postprocessor.");
                ExposedInitializator.UpdateReferences();
            }
        }

         
    }
}