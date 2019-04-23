using System.Collections.Generic;
using Exposed.Editor.Config;
using Exposed.Editor.DI;
using Autofac;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor
{
    [CustomEditor(typeof(ExposedConfiguration))]
    public class ExposedConfigurationEditor : UnityEditor.Editor
    {
        private IConfigFieldRenderer _configFieldRenderer;

        private ExposedConfiguration _exposedConfiguration;

        public void OnEnable()
        {
            _configFieldRenderer = ExposedEditorDi.Instance.Container.Resolve<IConfigFieldRenderer>();

            _exposedConfiguration = (ExposedConfiguration) target;
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(_exposedConfiguration, "Exposed Configuration Change");

            string propertyForDeletion = null;
            foreach (KeyValuePair<string, ExposedPropertyConfiguration> keyValuePair in _exposedConfiguration.ScriptConfigurationManager.Properties)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(45)))
                {
                    propertyForDeletion = keyValuePair.Key;
                    break;
                }
                _configFieldRenderer.RenderConfigField(null, keyValuePair.Value, keyValuePair.Key);
            }
            if (propertyForDeletion != null)
            {_exposedConfiguration.ScriptConfigurationManager.RemovePropertyConfiguration(propertyForDeletion);}

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_exposedConfiguration);
            }

        }
    }
}