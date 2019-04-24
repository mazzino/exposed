using System;
using System.Collections.Generic;
using System.Reflection;
using Exposed.Config;
using Exposed.DI;
using Exposed.Utils;
using Autofac;
using UnityEngine;

namespace Exposed
{
    public class ExposedReferences : MonoBehaviour, ISerializationCallbackReceiver
    {
        private Dictionary<Component, ExposedConfiguration> _properties = new Dictionary<Component, ExposedConfiguration>();
        //public Dictionary<Object, ExposedConfiguration> Properties { get { return _properties;} }

        //structures for serializing _componentProperties
        [SerializeField] private List<Component> _componentKeys = new List<Component>();
        [SerializeField] private List<ExposedConfiguration> _componentValues = new List<ExposedConfiguration>();

        [SerializeField] private bool _preInitializationEnabled = true;
        public bool PreInitializationEnabled { get { return _preInitializationEnabled; } set { _preInitializationEnabled = value; } }
        [SerializeField] private bool _lateUpdate;
        public bool LateUpdate { get { return _lateUpdate; } set { _lateUpdate = value; } }

        public void OnBeforeSerialize()
        {
            _componentKeys.Clear();
            _componentValues.Clear();

            foreach (var kvp in _properties)
            {
                _componentKeys.Add(kvp.Key);
                _componentValues.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _properties = new Dictionary<Component, ExposedConfiguration>();
            for (int i = 0; i != Math.Min(_componentKeys.Count, _componentValues.Count); i++)
            {
                //sometimes null happened because of undo system (it can remove component without ExposedReferencesEditor being in focus)
                if (!ReferenceEquals(_componentKeys[i], null))
                {
                    _properties.Add(_componentKeys[i], _componentValues[i]);
                }
            }
        }

        public ExposedConfiguration GetConfigForScript(Component scriptObject)
        {
            if (!_properties.ContainsKey(scriptObject))
            {
                _properties[scriptObject] = null;
            }

            return _properties[scriptObject];
        }

        public void SetConfigForScriptObject(Component component, ExposedConfiguration configuration)
        {
            _properties[component] = configuration;
        }

        public void Awake()
        {
            if (!LateUpdate && (!ExposedScriptConfigurationManagerAsset.Instance.PreInitializationEnabled || !PreInitializationEnabled))
            {
                ExposedLogger.Info(name + " - without caching awake", this);
                UpdateReferences();
            }
        }

        public void Start()
        {
            if (LateUpdate && (!ExposedScriptConfigurationManagerAsset.Instance.PreInitializationEnabled || !PreInitializationEnabled))
            {
                ExposedLogger.Info(name + " - without caching start", this);
                UpdateReferences();
            }
        }

        public void UpdateReferences()
        {
            ExposedLogger.Info(name + " - updating references", this);
            var componentObtainer = ExposedApplicationDi.Instance.Container.Resolve<IComponentObtainer>();
            var fieldsProcessor = ExposedApplicationDi.Instance.Container.Resolve<IFieldsProcessor>();
            var fieldsObtainer = ExposedApplicationDi.Instance.Container.Resolve<IFieldsObtainer>();

            List<ExposedMapping> scriptMappings = componentObtainer.GetComponentMappingsOnSameGameObject(this);

            //Debug.LogError(gameObject, gameObject);
            foreach (var scriptMapping in scriptMappings)
            {
                ExposedConfiguration configuration = GetConfigForScript(scriptMapping.AffectedComponent);
                if (configuration != null)
                {
                    List<FieldInfo> fields = fieldsObtainer.GetReferenceFieldsForType(scriptMapping.Type);
                    fieldsProcessor.ProcessFieldsForType(fields, scriptMapping.AffectedComponent, configuration);
                }
            }
        }

#if UNITY_EDITOR
        public void Reset()
        {
            MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();

            foreach (var monoBehaviour in components)
            {
                Type monoBehaviourType = monoBehaviour.GetType();

                FieldInfo[] fields = monoBehaviourType.GetFields();
                foreach (var fieldInfo in fields)
                {
                    if (fieldInfo.FieldType == typeof (ExposedConfiguration))
                    {
                        SetConfigForScriptObject(monoBehaviour, (ExposedConfiguration) fieldInfo.GetValue(monoBehaviour));
                        break;
                    }
                }
            }

        }
#endif
    }
}