using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbivoreController : MonoBehaviour
{
    private CreatureController myController;
    private string myFoodType = "Grass";
    public enum HerbivoreTypes
    {
        Sheep
    }

    private void Awake()
    {
        myController= GetComponent<CreatureController>();
        CreatureManager.Instance.Herbivores.Add(this);
        myController.myFood = myFoodType;
    }

    private void OnDestroy()
    {
        CreatureManager.Instance.Herbivores.Remove(this);
    }

}
