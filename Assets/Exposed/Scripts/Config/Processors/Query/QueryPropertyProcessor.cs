using System;
using UnityEngine;

namespace Exposed.Config.Processors.Query
{
    class QueryPropertyProcessor : PropertyProcessor
    {
        public override string GetUniqueId()
        {
            return "exposed-query";
        }

        public override string GetActionTypeLabel()
        {
            return "Exposed Query";
        }

        public override GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return affectedComponent.gameObject;
        }

        public override Component ProcessSingleComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            string query = propertyConfiguration.Query;
            return affectedComponent.gameObject.GetComponent(query);
        }

        public override GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return new[] {affectedComponent.gameObject};
        }

        public override Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            //string query = propertyConfiguration.Query;
            return affectedComponent.gameObject.GetComponents(fieldType);
        }
    }
}