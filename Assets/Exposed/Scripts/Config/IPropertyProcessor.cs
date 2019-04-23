using System;
using UnityEngine;

namespace Exposed.Config
{
    public interface IPropertyProcessor
    {
        string GetUniqueId();
        string GetActionTypeLabel();
        Component ProcessSingleComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration);
        Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration);
        GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration);
        GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration);
        void RenderAdditionalGui(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration);
    }
}