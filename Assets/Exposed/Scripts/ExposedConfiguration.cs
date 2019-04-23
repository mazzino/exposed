using UnityEngine;

namespace Exposed
{
    public class ExposedConfiguration : ScriptableObject
    {
        [SerializeField] private ExposedPropertyConfigurationManager _exposedScriptConfigurationManager 
            = new ExposedPropertyConfigurationManager();
        public ExposedPropertyConfigurationManager ScriptConfigurationManager { get { return _exposedScriptConfigurationManager; } }
         
    }
}