using System;
using System.Collections.Generic;
using Exposed.Config.Processors.AdditionalData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Exposed.Config.Processors.Core
{
    class ObjectOfTypePropertyProcessor : PropertyProcessor
    {
        public override string GetUniqueId()
        {
            return "exposed-object_of_type";
        }

        public override string GetActionTypeLabel()
        {
            return "OBJECT OF TYPE - Components or GameObjects with class of type";
        }

        public override GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return FindObjectOfType<GameObject>(affectedComponent, fieldType, propertyConfiguration);
        }

        public override Component ProcessSingleComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return FindObjectOfType<Component>(affectedComponent, fieldType, propertyConfiguration);
        }

        private T FindObjectOfType<T>(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
            where T : Object
        {
            ObjectOfTypeData objectOfTypeData = (ObjectOfTypeData)propertyConfiguration.AdditionalData;
            //without filtering by name
            if (!objectOfTypeData.FilterByName)
            {
                return Object.FindObjectOfType(fieldType) as T;
            }

            string currentNameFilter = GetCurrentNameFilter(affectedComponent.name, objectOfTypeData);
            Object[] objects = Object.FindObjectsOfType(fieldType);
            foreach (var unityObject in objects)
            {
                if (unityObject.name == currentNameFilter)
                {
                    return unityObject as T;
                }
            }
            return null;
        }

        public override GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return FindObjectsOfType<GameObject>(affectedComponent, fieldType, propertyConfiguration);
        }

        public override Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            return FindObjectsOfType<Component>(affectedComponent, fieldType, propertyConfiguration);
        }

        private T[] FindObjectsOfType<T>(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
            where T : Object
        {
            ObjectOfTypeData objectOfTypeData = (ObjectOfTypeData)propertyConfiguration.AdditionalData;
            T[] objects = Object.FindObjectsOfType(fieldType) as T[];
            //without filtering by name
            if (!objectOfTypeData.FilterByName)
            {
                return objects;
            }

            if (objects == null)
            {
                return null;
            }

            string currentNameFilter = GetCurrentNameFilter(affectedComponent.name, objectOfTypeData);
            List<T> filteredObjects = new List<T>();
            foreach (var unityObject in objects)
            {
                if (unityObject.name == currentNameFilter)
                {
                    filteredObjects.Add(unityObject);
                }
            }
            return filteredObjects.ToArray();
        }

#if UNITY_EDITOR
        public override void RenderAdditionalGui(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration)
        {
            if (!(propertyConfiguration.AdditionalData is ObjectOfTypeData))
            {
                propertyConfiguration.AdditionalData = new ObjectOfTypeData();
            }
            ObjectOfTypeData objectOfTypeData = (ObjectOfTypeData)propertyConfiguration.AdditionalData;

            //beginHorizontal is in code before
            objectOfTypeData.FilterByName = UnityEditor.EditorGUILayout.ToggleLeft("Filter by name", objectOfTypeData.FilterByName, GUILayout.Width(100));
            UnityEditor.EditorGUI.BeginDisabledGroup(!objectOfTypeData.FilterByName);
            objectOfTypeData.NameFilter = UnityEditor.EditorGUILayout.TextField(objectOfTypeData.NameFilter);
            UnityEditor.EditorGUILayout.EndHorizontal();

            UnityEditor.EditorGUILayout.BeginHorizontal();
            objectOfTypeData.AddCurrentNamePrefix = UnityEditor.EditorGUILayout.ToggleLeft("Add gameObject name as prefix", 
                objectOfTypeData.AddCurrentNamePrefix, GUILayout.Width(200));

            if (affectedComponent != null)
            {
                string usedFilter = !objectOfTypeData.FilterByName
                    ? ""
                    : GetCurrentNameFilter(affectedComponent.name, objectOfTypeData);
                UnityEditor.EditorGUILayout.EndHorizontal();
                UnityEditor.EditorGUILayout.BeginHorizontal();
                UnityEditor.EditorGUILayout.LabelField("Used filter: " + usedFilter);
            }
            //endHorizontal is in code after

            UnityEditor.EditorGUI.EndDisabledGroup();
        }
#endif

        /// <summary>
        /// Returning just name filter or name filter prefixed with gameobject name, according to configuration
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <param name="propertyConfiguration"></param>
        /// <returns></returns>
        private string GetCurrentNameFilter(string gameObjectName, ObjectOfTypeData propertyConfiguration)
        {
            return propertyConfiguration.AddCurrentNamePrefix
                ? gameObjectName + propertyConfiguration.NameFilter
                : propertyConfiguration.NameFilter;
        }
    }
}