using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager Instance;

    public List<GameObject> Carnivores = new List<GameObject>();
    public List<GameObject> Herbivores = new List<GameObject>();

    public Vector3 centerPoint= Vector3.zero;
    public float radiusBounds = 30f;

    public bool ongoing = false;
    
    private void Awake()
    {
        Instance = this;
    }
    

}
