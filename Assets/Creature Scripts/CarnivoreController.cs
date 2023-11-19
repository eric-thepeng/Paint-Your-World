using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivoreController : MonoBehaviour
{
    private CreatureController myController;
    public int carnivoreRanking; //bigger number carnivore eat smaller number on collision
    private string myFoodType = "Prey";
    public enum CarnivoreTypes
    {
        Wolf,
        Fox
    }
    private void Update()
    {
    }

    private void Awake()
    {
        myController= GetComponent<CreatureController>();
        CreatureManager.Instance.Carnivores.Add(this);
        myController.myFood = myFoodType;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(myFoodType))
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnDestroy()
    {
        CreatureManager.Instance.Carnivores.Remove(this);
    }

}
