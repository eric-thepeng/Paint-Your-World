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
    public bool startedBehavior = false;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if(startedBehavior)
        {
            StartCreatureBehavior();
        }
    }
    public void StartCreatureBehavior()
    {
        foreach(GameObject obj in Carnivores)
        {
            obj.GetComponent<CreatureMovement>().enabled = true;
        }
        foreach(GameObject obj in Herbivores)
        {
            obj.GetComponent<CreatureMovement>().enabled = true;
        }
    }


}
