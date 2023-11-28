using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private string myFood;
    [SerializeField] private string myPredator;
    [SerializeField] private string myMate;
    [SerializeField] private float hungerIncrease = 10f;

    private CreatureReproducible babyMaker;
    private CreatureHunger hunger;
    private CreatureDestructible destroyer;
    private void Awake()
    {
        babyMaker= GetComponent<CreatureReproducible>();
        hunger = GetComponent<CreatureHunger>();
        destroyer = GetComponent<CreatureDestructible>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(myMate))
        {
            if (!col.GetComponent<CreatureReproducible>().justSpawnedBaby)
            {
                babyMaker.MakeBaby();
            }
        }
        else if (col.gameObject.CompareTag(myFood))
        {
            hunger.HungerGoUp(hungerIncrease);
        }
        else if (col.gameObject.CompareTag(myPredator))
        {
            destroyer.DestroyObject();
        }
    }
}
