using UnityEngine;

namespace Exposed
{
    public class ExposedRegisterHelper : MonoBehaviour
    {
        private ExposedRegister _exposedRegister;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        public void Initialize(ExposedRegister exposedRegister)
        {
            _exposedRegister = exposedRegister;
        }

        public void OnApplicationPause()
        {
            _exposedRegister.SavePermanentObjects();
        }

        public void OnApplicationQuit()
        {
            _exposedRegister.SavePermanentObjects();
        }

    }
}