using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Exposed
{
    [Serializable]
    public class ExposedPropertyConfiguration : ISerializationCallbackReceiver
    {
        [SerializeField] private bool _enabled;
        public bool Enabled { get {  return _enabled;} set { _enabled = value; } }

        [SerializeField] private ExposedSearchType _searchType = ExposedSearchType.Core;
        public ExposedSearchType SearchType { get { return _searchType;} set { _searchType = value; }}

        [SerializeField] private string _operationType = "";
        public string OperationType { get { return _operationType; } set { _operationType = value; } }

        [SerializeField] private string _query = "";
        public string Query { get { return _query; } set { _query = value; } }

        [SerializeField] private object _additionalData = "";
        public object AdditionalData { get { return _additionalData; } set { _additionalData = value; } }

        [SerializeField] private string _additionalDataString;

        public void OnBeforeSerialize()
        {
            var b = new BinaryFormatter();
            var m = new MemoryStream();
            b.Serialize(m, _additionalData);
            _additionalDataString = Convert.ToBase64String(m.GetBuffer());
        }

        public void OnAfterDeserialize()
        {
            var b = new BinaryFormatter();
            var memoryStreamPrefsData = new MemoryStream(Convert.FromBase64String(_additionalDataString));
            _additionalData = b.Deserialize(memoryStreamPrefsData);
        }
    }
}