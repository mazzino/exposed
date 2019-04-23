using UnityEngine;

namespace Exposed.Editor.Config
{
    public interface IConfigFieldRenderer
    {
        void RenderConfigField(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration, string fieldId, string typeName=null);
    }
}