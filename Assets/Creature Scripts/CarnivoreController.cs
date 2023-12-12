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
        //myController.myFood = myFoodType;

    }
    private void Start()
    {
        GetCreatureManager().Carnivores.Add(this.gameObject);
    }

    private CreatureManager GetCreatureManager()
    {
        return GetComponent<CreatureController>().creatureMan;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(myFoodType))
        {
            //Debug.Log("eat");
            Destroy(collision.gameObject);
            myController.creatureHunger.HungerGoUp(collision.GetComponent<CreatureController>().calVal);
            myController.creatureMovement.StartCoroutine(myController.creatureMovement.CreatureEating());
        }
    }
    private void OnDestroy()
    {
        GetCreatureManager().Carnivores.Remove(this.gameObject);
    }

}
