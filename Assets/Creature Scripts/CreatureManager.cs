using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager Instance;

    public List<GameObject> Carnivores = new List<GameObject>();
    public List<GameObject> Herbivores = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

}
