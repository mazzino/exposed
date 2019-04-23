
using System;
using UnityEngine;

namespace Exposed.Prefs
{
    [Serializable]
    public class ExposedInt
    {
        [SerializeField] private string _prefId = "key";

        [SerializeField] private int _value;
        public int Value {
            get
            {
                if (PrefsRegister.Instance.ContainsPref(_prefId))
                {
                    _value = PlayerPrefs.GetInt(_prefId);
                }
                return _value;
            }
            set
            {
                PlayerPrefs.SetInt(_prefId, value);
                _value = value;
            } 
        }
    }
}