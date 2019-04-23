using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Exposed.Config
{
    public interface IFieldsProcessor
    {
        void ProcessFieldsForType(List<FieldInfo> fields, Component affectedComponent, ExposedConfiguration configuration);
    }
}