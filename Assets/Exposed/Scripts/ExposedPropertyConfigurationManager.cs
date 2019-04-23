using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exposed
{
    [Serializable]
    public class ExposedPropertyConfigurationManager : ISerializationCallbackReceiver
    {
        private Dictionary<string, ExposedPropertyConfiguration> _properties = new Dictionary<string, ExposedPropertyConfiguration>();
        public Dictionary<string, ExposedPropertyConfiguration> Properties { get { return _properties;} }

        //structures for serializing _componentProperties
        [SerializeField] private List<string> _componentKeys = new List<string>();
        [SerializeField] private List<ExposedPropertyConfiguration> _componentValues = new List<ExposedPropertyConfiguration>();

        public void OnBeforeSerialize()
        {
            _componentKeys.Clear();
            _componentValues.Clear();

            if (_properties != null)
            {
                foreach (var kvp in _properties)
                {
                    _componentKeys.Add(kvp.Key);
                    _componentValues.Add(kvp.Value);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            _properties = new Dictionary<string, ExposedPropertyConfiguration>();
            for (int i = 0; i != Math.Min(_componentKeys.Count, _componentValues.Count); i++)
            {
                _properties.Add(_componentKeys[i], _componentValues[i]);
            }
        }

        public ExposedPropertyConfiguration GetPropertyConfiguration(string propertyId)
        {
            if (!_properties.ContainsKey(propertyId))
            {
                _properties[propertyId] = new ExposedPropertyConfiguration();
            }
            return _properties[propertyId];
        }

        public void SetPropertyConfiguration(string propertyId, ExposedPropertyConfiguration configuration)
        {
            _properties[propertyId] = configuration;
        }

        public void RemovePropertyConfiguration(string propertyId)
        {
            _properties.Remove(propertyId);
        }

        /*public void Merge(ExposedPropertyConfigurationManager exposedPropertyConfigurationManager)
        {
            foreach (KeyValuePair<ComponentPropertyId, ExposedPropertyConfiguration> kvp in exposedPropertyConfigurationManager.Properties)
            {
                _properties[kvp.Key] = kvp.Value;
            }
        }*/
         
    }
}