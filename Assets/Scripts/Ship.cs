﻿using System.Collections.Generic;
using Exposed.Prefs;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public ExposedInt Score;
    public ExposedInt Counter;

    public Cannon Cannon;
    public Cannon[] Cannons;
    public GameObject CannonGameObject;
    public List<GameObject> CannonGameObjects;

    public void Awake()
    {
        /*Debug.LogError(Cannon, Cannon);
        foreach (var cannon in Cannons)
        {
            Debug.LogError("array:" + cannon, cannon);    
        }
        Debug.LogError(CannonGameObject, CannonGameObject);
        foreach (var cannon in CannonGameObjects)
        {
            Debug.LogError("list:" + cannon, cannon);    
        }*/

        //GunMechanism = new AggressiveGunMechanism();

    }
}