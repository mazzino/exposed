using System.Collections.Generic;
using System.Reflection;
using Exposed.Utils;
using UnityEngine;

namespace Exposed.Config
{
    class FieldsProcessor : IFieldsProcessor
    {
        private readonly IPropertyProcessorManager _propertyProcessorManager;
        private readonly IArrayTypeUtils _arrayTypeUtils;

        public FieldsProcessor(IPropertyProcessorManager propertyProcessorManager,
            IArrayTypeUtils arrayTypeUtils)
        {
            _propertyProcessorManager = propertyProcessorManager;
            _arrayTypeUtils = arrayTypeUtils;
        }

        public void ProcessFieldsForType(List<FieldInfo> fields ,Component affectedComponent,
            ExposedConfiguration configuration)
        {
            foreach (var field in fields)
            {
                ProcessField(field, affectedComponent, configuration);    
            }
        }

        private void ProcessField(FieldInfo field, Component affectedComponent, ExposedConfiguration configuration)
        {
            ExposedPropertyConfiguration propertyConfiguration = configuration.ScriptConfigurationManager.GetPropertyConfiguration(field.Name);
            if (propertyConfiguration.Enabled)
            {
                //singlefield
                if (!_arrayTypeUtils.IsSupportedArrayType(field.FieldType))
                {
                    _propertyProcessorManager.ProcessSingleFieldWithProcessor(propertyConfiguration, 
                        affectedComponent, field, field.FieldType);
                }
                //arrayfield
                else
                {
                    _propertyProcessorManager.ProcessArrayFieldWithProcessor(propertyConfiguration, 
                        affectedComponent, field, _arrayTypeUtils.GetElementTypeFromArray(field.FieldType));
                }
            }
        }
    }
}