using System.Collections.Generic;
using UnityEngine;

namespace Exposed.Config
{
    public interface IComponentObtainer
    {
        List<ExposedMapping> GetComponentMappingsOnSameGameObject(Component component);
    }
}