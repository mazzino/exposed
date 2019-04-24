using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Exposed.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Exposed.Config
{
    class PropertyProcessorManager : IPropertyProcessorManager
    {
        private readonly IArrayTypeUtils _arrayTypeUtils;
        private readonly Dictionary<string, IPropertyProcessor> _processors = new Dictionary<string, IPropertyProcessor>();

        private readonly int[] _operationTypes; 
        private readonly string[] _operationIds;
        private readonly string[] _operationLabels;

        private readonly int[] _customOperationTypes; 
        private readonly string[] _customOperationIds;
        private readonly string[] _customOperationLabels;

        private readonly Dictionary<string, int> _operationNamesMapping;

        public PropertyProcessorManager(IEnumerable<IPropertyProcessor> corePropertyProcessors, 
            IEnumerable<IPropertyProcessor> customPropertyProcessors,
            IPropertyProcessor queryProcessor,
            IArrayTypeUtils arrayTypeUtils)
        {
            _arrayTypeUtils = arrayTypeUtils;
            HashSet<IPropertyProcessor> coreProcessors = new HashSet<IPropertyProcessor>();
            HashSet<IPropertyProcessor> customProcessors = new HashSet<IPropertyProcessor>();

            //add query processor
            _processors[queryProcessor.GetUniqueId()] = queryProcessor;
            ProcessProcessors(corePropertyProcessors, coreProcessors);
            ProcessProcessors(customPropertyProcessors, customProcessors);

            _operationNamesMapping = new Dictionary<string, int>();

            _operationTypes = new int[coreProcessors.Count];
            _operationIds = new string[coreProcessors.Count];
            _operationLabels = new string[coreProcessors.Count];

            _customOperationTypes = new int[customProcessors.Count];
            _customOperationIds = new string[customProcessors.Count];
            _customOperationLabels = new string[customProcessors.Count];

            int counter = 0;
            foreach (IPropertyProcessor processor in coreProcessors)
            {
                _operationTypes[counter] = counter;
                _operationIds[counter] = processor.GetUniqueId();
                _operationLabels[counter] = processor.GetActionTypeLabel();
                _operationNamesMapping[_operationIds[counter]] = counter;
                counter++;
            }
            counter = 0;
            foreach (IPropertyProcessor processor in customProcessors)
            {
                _customOperationTypes[counter] = counter;
                _customOperationIds[counter] = processor.GetUniqueId();
                _customOperationLabels[counter] = processor.GetActionTypeLabel();
                _operationNamesMapping[_customOperationIds[counter]] = counter;
                counter++;
            }
        }

        private void ProcessProcessors(IEnumerable<IPropertyProcessor> propertyProcessors, HashSet<IPropertyProcessor> processors)
        {
            foreach (var propertyProcessor in propertyProcessors)
            {
                string processorId = propertyProcessor.GetUniqueId();
                if (!_processors.ContainsKey(processorId))
                {
                    _processors[processorId] = propertyProcessor;
                    processors.Add(propertyProcessor);
                }
                else
                {
                    Debug.LogError(
                        string.Format("Processor id <b>{0}</b> already exists, choose different id for class <b>{1}</b>.",
                            processorId, propertyProcessor.GetType().Name));
                }
            }
        }

        public void ProcessSingleFieldWithProcessor(ExposedPropertyConfiguration propertyConfiguration, Component affectedComponent, FieldInfo field, Type fieldType)
        {
            string operationType = propertyConfiguration.OperationType;
            if (_processors.ContainsKey(operationType))
            {
                IPropertyProcessor propertyProcessor = _processors[operationType];

                Object value = null;
                if (fieldType == typeof (GameObject))
                {
                    value = propertyProcessor.ProcessSingleGameObjectField(affectedComponent, fieldType, propertyConfiguration);
                }
                else if (fieldType.IsSubclassOf(typeof (Component)))
                {
                    value = propertyProcessor.ProcessSingleComponentField(affectedComponent, fieldType, propertyConfiguration);
                }
                field.SetValue(affectedComponent, value);
            }
        }

        public void ProcessArrayFieldWithProcessor(ExposedPropertyConfiguration propertyConfiguration, Component affectedComponent, FieldInfo field, Type fieldType)
        {
            string operationType = propertyConfiguration.OperationType;
            if (_processors.ContainsKey(operationType))
            {
                IPropertyProcessor propertyProcessor = _processors[operationType];

                Object[] values = {};

                if (fieldType == typeof (GameObject))
                {
                    values = propertyProcessor.ProcessArrayGameObjectField(
                        affectedComponent, fieldType, propertyConfiguration);
                }
                else if (fieldType.IsSubclassOf(typeof (Component)))
                {
                    values = propertyProcessor.ProcessArrayComponentField(
                        affectedComponent, fieldType, propertyConfiguration);
                }

                if (values != null)
                {
                    if (_arrayTypeUtils.IsArray(field.FieldType))
                    {
                        Array referencedObjectsArray = Array.CreateInstance(fieldType, values.Length);
                        for (int i = 0; i < referencedObjectsArray.Length; i++)
                        {
                            referencedObjectsArray.SetValue(values[i], i);
                        }

                        field.SetValue(affectedComponent, referencedObjectsArray);
                    }
                    //else it should be list
                    else
                    {
                        var list = (IList) Activator.CreateInstance(field.FieldType);

                        foreach (var value in values)
                        {
                            list.Add(value);
                        }
                        field.SetValue(affectedComponent, list);
                    }
                }
                else
                {
                    field.SetValue(affectedComponent,null);
                }
            }
        }

        public void GetPreparedCoreStructuresForRendering(out int[] operationTypes, out string[] operationIds, 
            out string[] operationLabels, out Dictionary<string, int> operationNamesMapping)
        {
            operationTypes = _operationTypes;
            operationIds = _operationIds;
            operationLabels = _operationLabels;
            operationNamesMapping = _operationNamesMapping;
        }

        public void GetPreparedCustomStructuresForRendering(out int[] operationTypes, out string[] operationIds, 
            out string[] operationLabels, out Dictionary<string, int> operationNamesMapping)
        {
            operationTypes = _customOperationTypes;
            operationIds = _customOperationIds;
            operationLabels = _customOperationLabels;
            operationNamesMapping = _operationNamesMapping;
        }

        public void RenderAdditionalProcessorGui(ExposedPropertyConfiguration propertyConfiguration, Component affectedComponent)
        {
            if (_processors.ContainsKey(propertyConfiguration.OperationType))
            {
                _processors[propertyConfiguration.OperationType].RenderAdditionalGui(affectedComponent, propertyConfiguration);
            }
        }
    }
}