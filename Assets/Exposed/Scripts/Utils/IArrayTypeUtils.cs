using System;
using System.Collections;

namespace Exposed.Utils
{
    public interface IArrayTypeUtils
    {
        bool IsSupportedArrayType(Type type);
        Type GetElementTypeFromArray(Type arrayType);
        bool IsArray(Type type);
        bool IsList(Type type);
        string GetFormatedType(Type arrayType);
        int GetRealItemsCount(IEnumerable enumerable);
    }
}