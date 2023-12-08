using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager Instance;

    public List<GameObject> Carnivores = new List<GameObject>();
    public List<GameObject> Herbivores = new List<GameObject>();

    public bool ongoing = false;
    public bool startedBehavior = false;

    public float boundsRadius = 40f;
    public Vector3 boundsCenterPoint = Vector3.zero;
    public LayerMask mask;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mask = LayerMask.NameToLayer("Creatures");
    }
    private void Update()
    {
        if(startedBehavior)
        {
            StartCreatureBehavior();
        }
        
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.CircleCast(boundsCenterPoint, boundsRadius, Vector2.zero, Mathf.Infinity, mask);
        //if (hit.collider.GetComponent<CreatureMovement>() != null)
        //{
        //    hit.collider.GetComponent<CreatureMovement>().moveToCenter = false;
        //}
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boundsCenterPoint, boundsRadius);
    }
}
