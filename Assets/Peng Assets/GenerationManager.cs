using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    static GenerationManager instance;
    public static GenerationManager i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GenerationManager>();
            }
            return instance;
        }
    }

    public PlaceableIdentifier placingPlaceableIdentifier;

    public PlaceableIdentifier[] allPlacingPlaceableIdentifiers = new PlaceableIdentifier[5];
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) placingPlaceableIdentifier = allPlacingPlaceableIdentifiers[0];
        if (Input.GetKeyDown(KeyCode.Alpha2)) placingPlaceableIdentifier = allPlacingPlaceableIdentifiers[1];
        if (Input.GetKeyDown(KeyCode.Alpha3)) placingPlaceableIdentifier = allPlacingPlaceableIdentifiers[2];
        if (Input.GetKeyDown(KeyCode.Alpha4)) placingPlaceableIdentifier = allPlacingPlaceableIdentifiers[3];
        if (Input.GetKeyDown(KeyCode.Alpha5)) placingPlaceableIdentifier = allPlacingPlaceableIdentifiers[4];
    }
}
