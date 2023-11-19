using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    //[SerializeField] private string myFood;
    //[SerializeField] private string myPredator;
    //[SerializeField] private string myMate;
    [SerializeField] private float hungerIncrease = 10f;

    public string myFood;
    public CreatureManager creatureMan;

    public CreatureReproducible babyMaker;
    public CreatureHunger hunger;
    public CreatureDestructible destroyer;
    public CreatureMovement mover;

    private float foodDetectRadius = 3f;

    private bool foodColLastFrame = false;
    private bool foodColThisFrame = false;

    private void Awake()
    {
        creatureMan = CreatureManager.Instance;

        babyMaker = GetComponent<CreatureReproducible>();
        hunger = GetComponent<CreatureHunger>();
        destroyer = GetComponent<CreatureDestructible>();
        mover = GetComponent<CreatureMovement>();
    }
    private void Update()
    {
        foodColThisFrame= false;
        RaycastHit2D foodDetectHit = Physics2D.CircleCast(transform.position, foodDetectRadius, Vector2.zero, 0f);
        if(foodDetectHit.collider != null )
        {
            if(foodDetectHit.collider.gameObject.CompareTag(myFood))
            {
                foodColThisFrame = true;
                if (!foodColLastFrame)
                {
                    Debug.Log("move to food");
                    mover.StartCoroutine(mover.CreatureEating(foodDetectHit.collider.gameObject.transform.position));
                }
            }
            
        }



        foodColLastFrame = foodColThisFrame;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, foodDetectRadius);
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
