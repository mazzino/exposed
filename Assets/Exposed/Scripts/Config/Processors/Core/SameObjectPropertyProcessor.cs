using System;
using UnityEngine;

namespace Exposed.Config.Processors.Core
{
    class SameObjectPropertyProcessor : PropertyProcessor
    {
        public override string GetUniqueId()
        {
            return "exposed-sameobject";
        }

        public override string GetActionTypeLabel()
        {
            return "GAMEOBJECT - Components on same GameObject";
        }

        public override GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return affectedComponent.gameObject;
        }

        public override Component ProcessSingleComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return affectedComponent.gameObject.GetComponent(fieldType);
        }

        public override GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return new[]{affectedComponent.gameObject};
        }

        public override Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return affectedComponent.gameObject.GetComponents(fieldType);
        }

    }
}