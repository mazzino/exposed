using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Exposed.Editor.Config
{
    public interface IFieldsRenderer
    {
        void RenderFieldsForType(List<FieldInfo> fields, Component affectedComponent, ExposedConfiguration configuration);
    }
}