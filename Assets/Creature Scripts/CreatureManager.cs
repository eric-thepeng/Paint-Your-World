using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager Instance;

    public List<CarnivoreController> Carnivores = new List<CarnivoreController>();
    public List<HerbivoreController> Herbivores = new List<HerbivoreController>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreatureCollision()
    {

    }
}
