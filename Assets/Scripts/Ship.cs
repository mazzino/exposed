using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Cannon Cannon;
    public Cannon[] Cannons;
    public GameObject CannonGameObject;
    public List<GameObject> CannonGameObjects;

    public void Awake()
    {
        Debug.Log(Cannon, Cannon);
        foreach (var cannon in Cannons)
        {
            Debug.Log("array:" + cannon, cannon);    
        }
        Debug.Log(CannonGameObject, CannonGameObject);
        foreach (var cannon in CannonGameObjects)
        {
            Debug.Log("list:" + cannon, cannon);    
        }
    }
}
