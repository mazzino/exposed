using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Exposed.Utils;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor.Config
{
    class FieldsRenderer : IFieldsRenderer
    {
        private readonly IArrayTypeUtils _arrayTypeUtils;
        private readonly IConfigFieldRenderer _configFieldRenderer;

        public FieldsRenderer(IArrayTypeUtils arrayTypeUtils,
            IConfigFieldRenderer configFieldRenderer)
        {
            _arrayTypeUtils = arrayTypeUtils;
            _configFieldRenderer = configFieldRenderer;
        }

        public void RenderFieldsForType(List<FieldInfo> fields, Component affectedComponent, ExposedConfiguration configuration)
        {
            foreach (var field in fields)
            {
                RenderFieldForType(field, affectedComponent, configuration);
            }
        }

        private void RenderFieldForType(FieldInfo field, Component affectedComponent, ExposedConfiguration configuration)
        {
            string fieldId = field.Name;
            ExposedPropertyConfiguration propertyConfiguration = configuration.ScriptConfigurationManager.GetPropertyConfiguration(fieldId);

            EditorGUILayout.BeginHorizontal();
            RenderFieldReferencesInformation(field, affectedComponent);
            string typeName = _arrayTypeUtils.GetFormatedType(field.FieldType);

            _configFieldRenderer.RenderConfigField(affectedComponent, propertyConfiguration, fieldId, typeName);
        }


        private void RenderFieldReferencesInformation(FieldInfo field, Component affectedComponent)
        {
            var value = field.GetValue(affectedComponent);
            int count = 0;
            //value.ToString() -> sometimes AudioSource or Animator wasn't null but "null", but inspector showed always None
            if (value != null && value.ToString() != "null")
            {
                //if there is a value and the field is array, it has to be nonempty
                if (_arrayTypeUtils.IsArray(field.FieldType))
                {
                    Array array = (Array) value;
                    count = _arrayTypeUtils.GetRealItemsCount(array);
                }
                //else if it's list
                else if (_arrayTypeUtils.IsList(field.FieldType))
                {
                    IList list = (IList)value;
                    count = _arrayTypeUtils.GetRealItemsCount(list);
                }
                //if it's field or object structure, the reference is ok
                else
                {
                    count = 1;
                }
            }

            GUIStyle guiStyle = new GUIStyle(EditorStyles.boldLabel);
            if (count == 0)
            {
                guiStyle.normal.textColor = Color.red;
            }

            EditorGUILayout.LabelField(count+"x", guiStyle, GUILayout.Width(25));
        }
    }
}