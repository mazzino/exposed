using System;
using System.Collections;
using System.Collections.Generic;

namespace Exposed.Utils
{
    class ArrayTypeUtils : IArrayTypeUtils
    {
        public bool IsSupportedArrayType(Type type)
        {
            return IsArray(type) || IsList(type);
        }

        public Type GetElementTypeFromArray(Type arrayType)
        {
            return IsArray(arrayType) ? arrayType.GetElementType() : arrayType.GetGenericArguments()[0];
        }

        public bool IsArray(Type type)
        {
            return type.IsArray;
        }

        public bool IsList(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof (List<>));
        }

        public string GetFormatedType(Type type)
        {
            if (IsArray(type))
            {
                return type.Name;
            }
            if (IsList(type))
            {
                return string.Format("List<{0}>", type.GetGenericArguments()[0].Name);
            }

            return type.Name;
        }

        /// <summary>
        /// Checks also whether the item in array is null or not
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public int GetRealItemsCount(IEnumerable enumerable)
        {
            int count = 0;

            foreach (var item in enumerable)
            {
                if (item != null)
                {
                    count++;
                }
            }
            return count;
        }
    }
}