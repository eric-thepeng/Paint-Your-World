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
}
