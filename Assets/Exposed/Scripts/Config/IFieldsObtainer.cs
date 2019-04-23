using System;
using System.Collections.Generic;
using System.Reflection;

namespace Exposed.Config
{
    public interface IFieldsObtainer
    {
        List<FieldInfo> GetReferenceFieldsForType(Type type);
    }
}