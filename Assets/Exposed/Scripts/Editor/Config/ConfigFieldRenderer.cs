using System.Collections.Generic;
using Exposed.Config;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor.Config
{
    class ConfigFieldRenderer : IConfigFieldRenderer
    {
        private readonly IPropertyProcessorManager _propertyProcessorManager;

        private readonly int[] _operationTypes; 
        private readonly string[] _operationIds;
        private readonly string[] _operationLabels;

        private readonly int[] _customOperationTypes; 
        private readonly string[] _customOperationIds;
        private readonly string[] _customOperationLabels;

        private readonly Dictionary<string, int> _operationNamesMapping;

        public ConfigFieldRenderer(IPropertyProcessorManager propertyProcessorManager)
        {
            _propertyProcessorManager = propertyProcessorManager;

            propertyProcessorManager.GetPreparedCoreStructuresForRendering(out _operationTypes, out _operationIds, 
                out _operationLabels, out _operationNamesMapping);
            propertyProcessorManager.GetPreparedCustomStructuresForRendering(out _customOperationTypes, out _customOperationIds, 
                out _customOperationLabels, out _operationNamesMapping);
        }

        public void RenderConfigField(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration, string fieldId, string typeName)
        {
            string toggleLabel = typeName != null ? string.Format("{0} ({1})", fieldId, typeName) : fieldId;

            propertyConfiguration.Enabled = EditorGUILayout.ToggleLeft(toggleLabel,
                propertyConfiguration.Enabled, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            EditorGUILayout.BeginVertical();
#else
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
#endif
            EditorGUI.BeginDisabledGroup(!propertyConfiguration.Enabled);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            propertyConfiguration.SearchType =
                (ExposedSearchType) EditorGUILayout.EnumPopup(propertyConfiguration.SearchType, GUILayout.Width(57));
            if (EditorGUI.EndChangeCheck())
            {
                //reset operation type in case of changing type of search
                propertyConfiguration.OperationType = "";
            }

            switch (propertyConfiguration.SearchType)
            {
                case ExposedSearchType.Custom:
                    RenderPropertyPopup(propertyConfiguration, _customOperationTypes, _customOperationIds,
                        _customOperationLabels);
                    break;
                case ExposedSearchType.Query:
                    propertyConfiguration.OperationType = "exposed-query";
                    EditorGUI.BeginDisabledGroup(true);
                    //propertyConfiguration.Query = EditorGUILayout.TextField(propertyConfiguration.Query);
                    EditorGUILayout.TextField("coming soon...");
                    EditorGUI.EndDisabledGroup();
                    break;
                //ExposedSearchType.Core
                default:
                    RenderPropertyPopup(propertyConfiguration, _operationTypes, _operationIds, _operationLabels);
                    break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _propertyProcessorManager.RenderAdditionalProcessorGui(propertyConfiguration, affectedComponent);
            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private void RenderPropertyPopup(ExposedPropertyConfiguration propertyConfiguration, 
            int[] operationTypes, string[] operationIds, string[] operationLabels)
        {
            int selectedItem = 0;
            if (_operationNamesMapping.ContainsKey(propertyConfiguration.OperationType))
            {
                selectedItem = _operationNamesMapping[propertyConfiguration.OperationType];
            }
            selectedItem = EditorGUILayout.IntPopup("", selectedItem, operationLabels, operationTypes);
            if (operationTypes.Length > 0)
            {
                propertyConfiguration.OperationType = operationIds[selectedItem];
            }
        }

    }
}