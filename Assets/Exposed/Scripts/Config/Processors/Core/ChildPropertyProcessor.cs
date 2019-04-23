using System;
using System.Collections.Generic;
using Exposed.Config.Processors.AdditionalData;
using UnityEngine;

namespace Exposed.Config.Processors.Core
{
    class ChildPropertyProcessor : PropertyProcessor
    {
        public override string GetUniqueId()
        {
            return "exposed-children";
        }

        public override string GetActionTypeLabel()
        {
            return "CHILDREN - Components or GameObjects in children";
        }

        public override GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            HierarchyData hierarchyData = (HierarchyData)propertyConfiguration.AdditionalData;

            //active - fastest, self included variant
            if (!hierarchyData.IncludeInactive && hierarchyData.IncludeSelf)
            {
                Transform transform = affectedComponent.GetComponentInChildren<Transform>();
                return transform != null ? transform.gameObject : null;
            }
            //nonactive included
            Transform[] transforms =  affectedComponent.gameObject.GetComponentsInChildren<Transform>(true);
            foreach (var component in transforms)
            {
                if (HierarchyData.PassedThroughConditions(component, affectedComponent, hierarchyData))
                {
                    return component.gameObject;
                }
            }
            return null;
        }

        public override Component ProcessSingleComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            HierarchyData hierarchyData = (HierarchyData)propertyConfiguration.AdditionalData;

            //active - fastest, self included variant
            if (!hierarchyData.IncludeInactive && hierarchyData.IncludeSelf)
            {
                return affectedComponent.gameObject.GetComponentInChildren(fieldType);
            }
            //nonactive included
            Component[] components =  affectedComponent.gameObject.GetComponentsInChildren(fieldType, true);
            foreach (var component in components)
            {
                if (HierarchyData.PassedThroughConditions(component,affectedComponent,hierarchyData))
                {
                    return component;
                }
            }
            return null;
        }

        public override GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            HierarchyData hierarchyData = (HierarchyData)propertyConfiguration.AdditionalData;

            Transform[] transforms = affectedComponent.gameObject.GetComponentsInChildren<Transform>(hierarchyData.IncludeInactive);
            List<GameObject> gameObjects = new List<GameObject>();

            foreach (var transform in transforms)
            {
                if (HierarchyData.PassedThroughConditions(transform, affectedComponent, hierarchyData))
                {
                    gameObjects.Add(transform.gameObject);
                }
            }
            return gameObjects.ToArray();
        }

        public override Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            HierarchyData hierarchyData = (HierarchyData)propertyConfiguration.AdditionalData;

            Component[] components = affectedComponent.gameObject.GetComponentsInChildren(fieldType, hierarchyData.IncludeInactive);
            if (hierarchyData.IncludeSelf)
            {
                return components;
            }

            List<Component> nonSelfComponents = new List<Component>();
            foreach (var component in components)
            {
                if (HierarchyData.PassedThroughConditions(component, affectedComponent, hierarchyData))
                {
                    nonSelfComponents.Add(component);
                }
            }
            return nonSelfComponents.ToArray();
        }

#if UNITY_EDITOR
        public override void RenderAdditionalGui(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            //inicialization of new data
            if (!(propertyConfiguration.AdditionalData is HierarchyData))
            {
                propertyConfiguration.AdditionalData = new HierarchyData();
            }
            HierarchyData hierarchyData = (HierarchyData)propertyConfiguration.AdditionalData;
            hierarchyData.IncludeInactive = UnityEditor.EditorGUILayout.ToggleLeft("Nonactive included", hierarchyData.IncludeInactive, 
                GUILayout.Width(130));
            hierarchyData.IncludeSelf = UnityEditor.EditorGUILayout.ToggleLeft("Self included", hierarchyData.IncludeSelf);
            UnityEditor.EditorGUILayout.EndHorizontal();
        }
#endif

    }
}