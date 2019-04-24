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
        Debug.LogError(Cannon, Cannon);
        foreach (var cannon in Cannons)
        {
            Debug.LogError("array:" + cannon, cannon);    
        }
        Debug.LogError(CannonGameObject, CannonGameObject);
        foreach (var cannon in CannonGameObjects)
        {
            Debug.LogError("list:" + cannon, cannon);    
        }
    }
}
