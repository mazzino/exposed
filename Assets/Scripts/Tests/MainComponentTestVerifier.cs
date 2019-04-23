using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    public class MainComponentTestVerifier : MonoBehaviour
    {
        public bool TestArrayOrder = true;

        public ComponentOne Component;
        public ComponentOne[] ArrayOfComponents;
        public List<ComponentOne> ListOfComponents;

        public GameObject ComponentGameObject;
        public GameObject[] ArrayOfGameObjects;
        public List<GameObject> ListOfGameObjects;

        private MainComponent _mainComponent;

        public void Awake()
        {
            _mainComponent = GetComponent<MainComponent>();

            PerformTest();
        }

        private void PerformTest()
        {
            Debug.Log("Test started:" + _mainComponent.gameObject.name, _mainComponent);
            if (TestArrayOrder)
            {
                TestSingle(Component, _mainComponent.Component);
                TestArray(ArrayOfComponents, _mainComponent.ArrayOfComponents);
                TestList(ListOfComponents, _mainComponent.ListOfComponents);
            }
            else
            {
                TestSingleOrderIndependent(Component, _mainComponent.Component);
                TestArrayOrderIndependent(ArrayOfComponents, _mainComponent.ArrayOfComponents);
                TestListOrderIndependent(ListOfComponents, _mainComponent.ListOfComponents);
            }

            if (TestArrayOrder)
            {
                TestSingle(ComponentGameObject, _mainComponent.ComponentGameObject);
                TestArray(ArrayOfGameObjects, _mainComponent.ArrayOfGameObjects);
                TestList(ListOfGameObjects, _mainComponent.ListOfGameObjects);
            }
            else
            {
                TestSingleOrderIndependent(ComponentGameObject, _mainComponent.ComponentGameObject);
                TestArrayOrderIndependent(ArrayOfGameObjects, _mainComponent.ArrayOfGameObjects);
                TestListOrderIndependent(ListOfGameObjects, _mainComponent.ListOfGameObjects);
            }
            Debug.Log("Test completed:" + _mainComponent.gameObject.name, _mainComponent);
        }

        private void TestSingle(Object firstObject, Object secondObject)
        {
            if (firstObject != secondObject)
            {
                Debug.LogError(string.Format("Objects aren't the same! <b>{0}</b>:<b>{1}</b>", 
                    firstObject.name, secondObject.name), _mainComponent);
            }
        }

        private void TestSingleOrderIndependent(Object firstObject, Object secondObject)
        {
            //in order independent variant, it's possible only to test whether the object was found or not
            if ((firstObject == null && secondObject != null) || (firstObject != null && secondObject == null))
            {
                Debug.LogError(string.Format("Objects aren't the same! <b>{0}</b>:<b>{1}</b>", 
                    firstObject.name, secondObject.name), _mainComponent);
            }
        }

        private void TestArray<T>(T[] firstArray, T[] secondArray)
            where T : Object
        {
            if (firstArray.Length != secondArray.Length)
            {
                Debug.LogError(string.Format("Arrays aren't the same lenght! <b>{0}</b>:<b>{1}</b>", firstArray, secondArray),
                    _mainComponent);
            }
            for (int i = 0; i < firstArray.Length; i++)
            {
                if (firstArray[i] != secondArray[i])
                {
                    Debug.LogError(string.Format("Array items aren't the same! <b>{0}</b>:<b>{1}</b>", firstArray[i], secondArray[i]),
                        _mainComponent);
                }
            }
        }

        private void TestArrayOrderIndependent<T>(T[] firstArray, T[] secondArray)
            where T : Object
        {
            if (firstArray.Length != secondArray.Length)
            {
                Debug.LogError(string.Format("Arrays aren't the same lenght! <b>{0}</b>:<b>{1}</b>", firstArray, secondArray),
                    _mainComponent);
            }
            for (int i = 0; i < firstArray.Length; i++)
            {
                if (!secondArray.Contains(firstArray[i]))
                {
                    Debug.LogError(string.Format("Array items aren't the same! Second array doesn't contain <b>{0}</b>", firstArray[i]),
                        _mainComponent);
                }
            }

            for (int i = 0; i < secondArray.Length; i++)
            {
                if (!firstArray.Contains(secondArray[i]))
                {
                    Debug.LogError(string.Format("Array items aren't the same! First array doesn't contain <b>{0}</b>", secondArray[i]),
                        _mainComponent);
                }
            }
        }

        private void TestList<T>(List<T> firstList, List<T> secondList)
            where T : Object
        {
            if (firstList.Count != secondList.Count)
            {
                Debug.LogError(string.Format("Lists aren't the same lenght! <b>{0}</b>:<b>{1}</b>", firstList, secondList),
                    _mainComponent);
            }
            for (int i = 0; i < firstList.Count; i++)
            {
                if (firstList[i] != secondList[i])
                {
                    Debug.LogError(string.Format("List items aren't the same! <b>{0}</b>:<b>{1}</b>", firstList[i], secondList[i]),
                        _mainComponent);
                }
            }
        }

        private void TestListOrderIndependent<T>(List<T> firstList, List<T> secondList)
            where T : Object
        {
            if (firstList.Count != secondList.Count)
            {
                Debug.LogError(string.Format("Lists aren't the same lenght! <b>{0}</b>:<b>{1}</b>", firstList, secondList),
                    _mainComponent);
            }
            for (int i = 0; i < firstList.Count; i++)
            {
                if (!secondList.Contains(firstList[i]))
                {
                    Debug.LogError(string.Format("List items aren't the same! Second list doesn't contain <b>{0}</b>", firstList[i]),
                        _mainComponent);
                }
            }
            for (int i = 0; i < secondList.Count; i++)
            {
                if (!firstList.Contains(secondList[i]))
                {
                    Debug.LogError(string.Format("List items aren't the same! First list doesn't contain <b>{0}</b>", secondList[i]),
                        _mainComponent);
                }
            }
        }
    }
}