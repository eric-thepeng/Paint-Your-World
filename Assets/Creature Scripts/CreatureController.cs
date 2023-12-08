using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    //[SerializeField] private string myFood;
    //[SerializeField] private string myPredator;
    [SerializeField] private string myMate;
    [SerializeField] private float hungerIncrease = 10f;
    public float calVal;

    public string myFood = "Predator";
    public CreatureManager creatureMan;

    public CreatureReproducible creatureReproducible;
    public CreatureHunger creatureHunger;
    public CreatureDestructible creatureDestructible;
    public CreatureMovement creatureMovement;
    public CreatureHungerBar creatureHungerBar;

    private float foodDetectRadius = 1f;

    private bool foodColThisFrame, foodColLastFrame;


    private void Awake()
    {

        creatureReproducible = GetComponent<CreatureReproducible>();
        creatureHunger = GetComponent<CreatureHunger>();
        creatureDestructible = GetComponent<CreatureDestructible>();
        creatureMovement = GetComponent<CreatureMovement>();
        creatureHungerBar= GetComponent<CreatureHungerBar>();
    }
    private void FixedUpdate()
    {
        foodColThisFrame= false;
        RaycastHit2D foodDetectHit = Physics2D.CircleCast(transform.position, foodDetectRadius, Vector2.zero, 0f);
        if(foodDetectHit.collider != null )
        {
            if(foodDetectHit.collider.gameObject.CompareTag(myFood))
            {
                foodColThisFrame = true;
                if(!foodColLastFrame)
                {
                    //Debug.Log(name + "move to food" + foodDetectHit.collider.name);
                    creatureMovement.foodFollower = foodDetectHit.collider.gameObject;
                    creatureMovement.startPos = transform.position;
                    creatureMovement.moveToFood = true;
                }
            }
            else
            {
                creatureMovement.moveToFood= false;
                creatureMovement.foodFollower = null;
            }
            
        }
        foodColLastFrame = foodColThisFrame;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, foodDetectRadius);
    }
    private void OnDestroy()
    {
        Destroy(creatureHungerBar.hungerBarInstance);
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.CompareTag(myMate))
    //    {
    //        if (!col.GetComponent<CreatureReproducible>().justSpawnedBaby)
    //        {
    //            babyMaker.MakeBaby();
    //        }
    //    }
    //    else if (col.gameObject.CompareTag(myFood))
    //    {
    //        hunger.HungerGoUp(hungerIncrease);
    //    }
    //    else if (col.gameObject.CompareTag(myPredator))
    //    {
    //        destroyer.DestroyObject();
    //    }
    //}
}
