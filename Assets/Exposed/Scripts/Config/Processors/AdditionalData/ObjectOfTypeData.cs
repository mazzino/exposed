using System;

namespace Exposed.Config.Processors.AdditionalData
{
    [Serializable]
    public class ObjectOfTypeData
    {
        public bool FilterByName;
        public bool AddCurrentNamePrefix;
        public string NameFilter;
    }
}