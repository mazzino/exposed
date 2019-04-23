using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    public class MainComponent : MonoBehaviour
    {
        public ComponentOne Component;
        public ComponentOne[] ArrayOfComponents;
        public List<ComponentOne> ListOfComponents;

        public GameObject ComponentGameObject;
        public GameObject[] ArrayOfGameObjects;
        public List<GameObject> ListOfGameObjects;

    }
}