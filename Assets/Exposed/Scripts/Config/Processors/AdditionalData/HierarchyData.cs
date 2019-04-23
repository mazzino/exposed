using System;
using UnityEngine;

namespace Exposed.Config.Processors.AdditionalData
{
    [Serializable]
    public class HierarchyData
    {
        public bool IncludeInactive;
        public bool IncludeSelf = true;

        public static bool PassedThroughConditions(Component component, Component affectedComponent, HierarchyData hierarchyData)
        {
            bool isActive = component.gameObject.activeInHierarchy;
            bool isSelf = component.gameObject == affectedComponent.gameObject;

            if (hierarchyData.IncludeInactive && !hierarchyData.IncludeSelf)
            {
                if (!isSelf)
                {
                    return true;
                }
            }
            if (!hierarchyData.IncludeInactive && !hierarchyData.IncludeSelf)
            {
                if (isActive && !isSelf)
                {
                    return true;
                }
            }
            if (hierarchyData.IncludeInactive && hierarchyData.IncludeSelf)
            {
                return true;
            }

            if (!hierarchyData.IncludeInactive && hierarchyData.IncludeSelf)
            {
                if (isActive)
                {
                    return true;
                }
            }

            return false;
        }
    }
}