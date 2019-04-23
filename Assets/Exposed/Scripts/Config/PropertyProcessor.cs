using System;
using UnityEngine;

namespace Exposed.Config
{
    public abstract class PropertyProcessor : IPropertyProcessor
    {
        public abstract string GetUniqueId();
        public abstract string GetActionTypeLabel();

        public abstract Component ProcessSingleComponentField(Component affectedComponent, Type fieldType,
            ExposedPropertyConfiguration propertyConfiguration);

        public abstract Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType,
            ExposedPropertyConfiguration propertyConfiguration);

        public abstract GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType,
            ExposedPropertyConfiguration propertyConfiguration);

        public abstract GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType,
            ExposedPropertyConfiguration propertyConfiguration);

        public virtual void RenderAdditionalGui(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration)
        {
        }
    }
}