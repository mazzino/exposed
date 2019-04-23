using System;
using System.Collections.Generic;
using System.Reflection;
using Exposed.Utils;
using UnityEngine;

namespace Exposed.Config
{
    class FieldsObtainer : IFieldsObtainer
    {
        private readonly IArrayTypeUtils _arrayTypeUtils;

        public FieldsObtainer(IArrayTypeUtils arrayTypeUtils)
        {
            _arrayTypeUtils = arrayTypeUtils;
        }

        public List<FieldInfo> GetReferenceFieldsForType(Type componentType)
        {
            List<FieldInfo> resultFields = new List<FieldInfo>();
            FieldInfo[] fields = componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                //ignore exposed configuration asset
                if (field.FieldType == typeof (ExposedConfiguration)) { continue;}

                if (((field.IsPrivate && Attribute.IsDefined(field, typeof(SerializeField)))
                     || (field.IsPublic))
                    //nesmi se zaroven jednat o readonly promennou a konstantu
                    && (!field.IsInitOnly) && (!field.IsLiteral))
                {
                    if (field.FieldType.IsSubclassOf(typeof (UnityEngine.Object)))
                    {
                        resultFields.Add(field);
                    }
                    //arrayfield
                    else if (_arrayTypeUtils.IsSupportedArrayType(field.FieldType) &&
                             _arrayTypeUtils.GetElementTypeFromArray(field.FieldType)
                                 .IsSubclassOf(typeof (UnityEngine.Object)))
                    {
                        resultFields.Add(field);
                    }
                }
            }

            return resultFields;
        }

    }
}
