using System.Collections.Generic;
using Exposed.Utils;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor
{
    [InitializeOnLoad]
    public class ExposedInitializator
    {
        static ExposedInitializator()
        {
            EditorApplication.playmodeStateChanged += CacheInstances;
        }

        private static void CacheInstances()
        {
            if (ExposedScriptConfigurationManagerAsset.Instance.EditorPreInitializationEnabled)
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    ExposedLogger.Info("Running EXPOSED playmode preprocessor.");
                    UpdateReferences();
                }
            }
            
        }

        public static void UpdateReferences()
        {
            //cleaning register for dynamic instances inicialization
            //ExposedRegister.Instance.CleanUp();

            ExposedLogger.Info("Updating EXPOSED references");
            ExposedReferences[] exposedReferences = Resources.FindObjectsOfTypeAll<ExposedReferences>();

            List<ExposedReferences> sceneConfigurations = new List<ExposedReferences>();

            foreach (ExposedReferences exposedReference in exposedReferences)
            {
                //only scene objects
                if (exposedReference.hideFlags == HideFlags.None)
                {
                    sceneConfigurations.Add(exposedReference);
                }
                //prefabs and others
                else
                {
                    //for prefabs, caching is always disabled (otherwise no references through rules will be found)
                    exposedReference.PreInitializationEnabled = false;
                    //EditorUtility.SetDirty(exposedReference);
                }
            }

            //because of local caching settings, prefabs has to be modified before scene instances
            foreach (var sceneConfiguration in sceneConfigurations)
            {
                sceneConfiguration.PreInitializationEnabled = true;
                sceneConfiguration.UpdateReferences();
                EditorUtility.SetDirty(sceneConfiguration);
            }
        }
    }
}