using System;
using UnityEngine;

namespace Exposed
{
    public struct ExposedMapping
    {
        public Type Type { get; private set; }
        public Component AffectedComponent { get; private set; }

        public ExposedMapping(Type type, Component affectedComponent) : this()
        {
            Type = type;
            AffectedComponent = affectedComponent;
        }
         
    }
}