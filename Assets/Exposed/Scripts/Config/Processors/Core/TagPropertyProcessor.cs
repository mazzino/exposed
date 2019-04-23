using System;
using System.Collections.Generic;
using Exposed.Config.Processors.AdditionalData;
using UnityEngine;

namespace Exposed.Config.Processors.Core
{
    class TagPropertyProcessor : PropertyProcessor
    {
        public override string GetUniqueId()
        {
            return "exposed-tag";
        }

        public override string GetActionTypeLabel()
        {
            return "TAG - Components or GameObjects with specified tag";
        }

        public override GameObject ProcessSingleGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            TagData tagData = (TagData)propertyConfiguration.AdditionalData;

            string tag = tagData.Tag;
            if (string.IsNullOrEmpty(tag)) { return null;}

            if (!tagData.FilterByGameObjectName)
            {
                GameObject findGameObjectWithTag = GameObject.FindGameObjectWithTag(tag);

                return findGameObjectWithTag;
            }

            //filtering by gameObject's name
            GameObject[] findGameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            foreach (var gameObject in findGameObjectsWithTag)
            {
                if (gameObject.name == affectedComponent.name)
                {
                    return gameObject;
                }
            }
            return null;
        }

        public override Component ProcessSingleComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            TagData tagData = (TagData)propertyConfiguration.AdditionalData;

            string tag = tagData.Tag;
            if (string.IsNullOrEmpty(tag)) { return null;}

            //filtering by gameObject's name
            GameObject[] findGameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            foreach (var gameObject in findGameObjectsWithTag)
            {
                Component component = gameObject.GetComponent(fieldType);
                if (component != null)
                {
                    if (!tagData.FilterByGameObjectName)
                    {
                        return component;
                    }
                    if (gameObject.name == affectedComponent.name)
                    {
                        return component;
                    }
                }
            }
            return null;
        }

        public override GameObject[] ProcessArrayGameObjectField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            TagData tagData = (TagData)propertyConfiguration.AdditionalData;

            string tag = tagData.Tag;
            if (string.IsNullOrEmpty(tag)) { return null;}

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

            if (!tagData.FilterByGameObjectName)
            {
                return gameObjects;
            }

            List<GameObject> resultGameObjects = new List<GameObject>();
            //filtering by gameObject's name
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == affectedComponent.name)
                {
                    resultGameObjects.Add(gameObject);                    
                }
            }
            return resultGameObjects.ToArray();
        }

        public override Component[] ProcessArrayComponentField(Component affectedComponent, Type fieldType, ExposedPropertyConfiguration propertyConfiguration)
        {
            GameObject[] objects = ProcessArrayGameObjectField(affectedComponent, fieldType, propertyConfiguration);

            if (objects == null) { return null;}

            List<Component> components = new List<Component>();
            foreach (var gameObject in objects)
            {
                components.AddRange(gameObject.GetComponents(fieldType));
            }
            return components.ToArray();
        }

#if UNITY_EDITOR
        public override void RenderAdditionalGui(Component affectedComponent, ExposedPropertyConfiguration propertyConfiguration)
        {
            if (!(propertyConfiguration.AdditionalData is TagData))
            {
                propertyConfiguration.AdditionalData = new TagData();    
            }
            TagData tagData = (TagData)propertyConfiguration.AdditionalData;

            tagData.Tag = UnityEditor.EditorGUILayout.TagField("Tag", tagData.Tag);

            UnityEditor.EditorGUILayout.EndHorizontal();
            tagData.FilterByGameObjectName = UnityEditor.EditorGUILayout.ToggleLeft("Filter by gameObject's name", 
                tagData.FilterByGameObjectName, GUILayout.Width(200));
            UnityEditor.EditorGUILayout.BeginHorizontal();

            //on config asset, there's no affected component -> no control label will be shown
            if (affectedComponent != null)
            {
                string usedFilter = !tagData.FilterByGameObjectName
                    ? ""
                    : affectedComponent.name;
                UnityEditor.EditorGUILayout.EndHorizontal();
                UnityEditor.EditorGUILayout.LabelField("Used filter: " + usedFilter);
                UnityEditor.EditorGUILayout.BeginHorizontal();
            }
        }
#endif
    }
}