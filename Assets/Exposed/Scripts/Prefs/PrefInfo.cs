using System;

namespace Assets.Exposed.Scripts.Prefs
{
    [Serializable]
    public struct PrefInfo
    {
        public PrefType PrefType;

        public PrefInfo(PrefType prefType)
        {
            PrefType = prefType;
        }
    }
}