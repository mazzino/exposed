using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Exposed
{
    //[CreateAssetMenu]
    public class ExposedRegister : ScriptableObject//, ISerializationCallbackReceiver
    {
        private static ExposedRegister _instance;
        public static ExposedRegister Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ExposedRegister>("ExposedRegister");
                    _instance.LoadPermanentObjects();
                    if (Application.isPlaying)
                    {
                        _instance.HelperGameObject = new GameObject {name = "_ExposedRegisterHelper"};
                        //adding helper gameobject responsible for saving data after application quit
                        var exposedRegisterHelper =_instance.HelperGameObject.AddComponent<ExposedRegisterHelper>();
                        exposedRegisterHelper.Initialize(_instance);
                    }
                    if (_instance == null)
                    {
                        Debug.LogError("Exposed register wasn't found.");
                    }
                }
                return _instance;
            }
        }

        public GameObject HelperGameObject;

        private bool _sceneRestartTriggered;
        public void TriggerSceneRestart()
        {
            if (!_sceneRestartTriggered)
            {
                SceneCleanUp();
                _sceneRestartTriggered = true;
            }
        }
        public void TriggerNull()
        {
            _sceneRestartTriggered = false;
        }

        private void SceneCleanUp()
        {
            _mappedSceneObjects.Clear();
        }

        private readonly Dictionary<Type, object> _mappedApplicationObjects = new Dictionary<Type,object>();
        private readonly Dictionary<Type, object> _mappedSceneObjects = new Dictionary<Type, object>();

        private Dictionary<Type, object> _mappedPermanentObjects = new Dictionary<Type, object>();

        public void SavePermanentObjects()
        {
            var b = new BinaryFormatter();
            var m = new MemoryStream();
            b.Serialize(m, _mappedPermanentObjects);
            PlayerPrefs.SetString("extension-exposed-register",Convert.ToBase64String(m.GetBuffer()));
        }

        public void LoadPermanentObjects()
        {
            var b = new BinaryFormatter();
            var memoryStreamPrefsData = new MemoryStream(Convert.FromBase64String(PlayerPrefs.GetString("extension-exposed-register")));
            object deserializedObject = b.Deserialize(memoryStreamPrefsData);
            _mappedPermanentObjects = (Dictionary<Type, object>) deserializedObject;
        }


        public object CreateAndRegisterApplicationInstance(Type instanceType)
        {
            if (!_mappedApplicationObjects.ContainsKey(instanceType))
            {
                object instance = Activator.CreateInstance(instanceType);
                _mappedApplicationObjects.Add(instanceType, instance);
                return instance;
            }

            return _mappedApplicationObjects[instanceType];
        }

        public object CreateAndRegisterSceneInstance(Type instanceType)
        {
            if (!_mappedSceneObjects.ContainsKey(instanceType))
            {
                object instance = Activator.CreateInstance(instanceType);
                _mappedSceneObjects.Add(instanceType, instance);
                return instance;
            }

            return _mappedSceneObjects[instanceType];
        }

        public object CreateAndRegisterPermanentInstance(Type instanceType)
        {
            if (!_mappedPermanentObjects.ContainsKey(instanceType))
            {
                object instance = Activator.CreateInstance(instanceType);
                _mappedPermanentObjects.Add(instanceType, instance);
                return instance;
            }

            return _mappedPermanentObjects[instanceType];
        }

        public object CreateNormalInstance(Type instanceType)
        {
            return Activator.CreateInstance(instanceType);
        }
    }
}