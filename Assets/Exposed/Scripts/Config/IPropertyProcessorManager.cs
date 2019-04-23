using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Exposed.Config
{
    public interface IPropertyProcessorManager
    {
        void ProcessSingleFieldWithProcessor(ExposedPropertyConfiguration propertyConfiguration, Component affectedComponent, FieldInfo field, Type fieldType);
        void ProcessArrayFieldWithProcessor(ExposedPropertyConfiguration propertyConfiguration, Component affectedComponent, FieldInfo field, Type fieldType);

        void GetPreparedCoreStructuresForRendering(out int[] operationTypes, out string[] operationIds,
            out string[] operationLabels, out Dictionary<string, int> operationNamesMapping);

        void GetPreparedCustomStructuresForRendering(out int[] operationTypes, out string[] operationIds,
            out string[] operationLabels, out Dictionary<string, int> operationNamesMapping);

        void RenderAdditionalProcessorGui(ExposedPropertyConfiguration propertyConfiguration, Component affectedComponent);
    }
}