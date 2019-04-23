using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Exposed.Scripts.Prefs;
using UnityEngine;

namespace Exposed
{
    [CreateAssetMenu]
    public class PrefsRegister : ScriptableObject, ISerializationCallbackReceiver
    {
        private static PrefsRegister _instance;
        public static PrefsRegister Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<PrefsRegister>("PrefsRegister");

                    if (_instance == null)
                    {
                        Debug.LogError("Exposed register wasn't found.");
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, PrefInfo> _prefs = new Dictionary<string, PrefInfo>();
        [SerializeField] private List<string> _prefKeys = new List<string>();
        [SerializeField] private List<PrefInfo> _prefValues = new List<PrefInfo>();

        public void OnBeforeSerialize()
        {
            _prefKeys.Clear();
            _prefValues.Clear();

            if (_prefs != null)
            {
                foreach (var kvp in _prefs)
                {
                    _prefKeys.Add(kvp.Key);
                    _prefValues.Add(kvp.Value);
                }
            }
        }
        public void OnAfterDeserialize()
        {
            _prefs = new Dictionary<string, PrefInfo>();
            for (int i = 0; i != Math.Min(_prefKeys.Count, _prefValues.Count); i++)
            {
                _prefs.Add(_prefKeys[i], _prefValues[i]);
            }
        }

        public void AddPref(string key, PrefInfo info)
        {
            _prefs.Add(key, info);
            _prefKeys.Add(key);
            _prefValues.Add(info);
        }

        public void RemovePref(string key)
        {
            int keyIndex =  _prefs.Keys.ToList().IndexOf(key);
            _prefs.Remove(key);

            _prefKeys.RemoveAt(keyIndex);
            _prefValues.RemoveAt(keyIndex);
        }

        public bool ContainsPref(string key)
        {
            return _prefs.ContainsKey(key);
        }

        public int FillArrayAndGetIndexOfKey(out string[] keysArray, string key)
        {
            keysArray = new string[_prefKeys.Count];

            int keyIndex = -1;
            for (int i = 0; i < _prefKeys.Count; i++)
            {
                keysArray[i] = _prefKeys[i];
                if (_prefKeys[i] == key)
                {
                    keyIndex = i;
                }
            }

            return keyIndex;
        }
    }
}