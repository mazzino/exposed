using System.Collections.Generic;
using UnityEngine;

namespace Exposed.Config
{
    class ComponentObtainer : IComponentObtainer
    {
        public List<ExposedMapping> GetComponentMappingsOnSameGameObject(Component component)
        {
            var scriptMappings = new List<ExposedMapping>();
            MonoBehaviour[] monoBehaviours = component.GetComponents<MonoBehaviour>();
            foreach (var monobehaviour in monoBehaviours)
            {
                if (monobehaviour != component)
                {
                    scriptMappings.Add(new ExposedMapping(monobehaviour.GetType(),
                        monobehaviour));
                }
            }

            return scriptMappings;
        }
    }
}