using Fungus;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTimeMin = 2f, waitTimeMax = 5f;
    [SerializeField] private float boundsY=5, boundsX=9;

    private Rigidbody2D rb;
    private bool moving = true;
    public GameObject foodFollower;
    public Vector3 foodPos;
    public bool moveToFood;
    public bool moveToCenter;
    private Vector3 moveTo;
    private Coroutine idleMove;
    private Coroutine waitFor;

    public Vector3 startPos;
    private Vector3 facing;
    private float boundsRadius = 4f;
    private Vector3 centerPoint = Vector3.zero;

    private CreatureController myController;

    private CreatureManager cm;

    private GameObject hungerBar;

    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        myController = GetComponent<CreatureController>();
    }
    private void Start()
    {
        hungerBar = GetComponent<CreatureHungerBar>().hungerBarInstance;
        idleMove = StartCoroutine(CreatureIdleMove());
        boundsRadius = GetCreatureManager().boundsRadius;
        centerPoint = GetCreatureManager().boundsCenterPoint;
    }

    public CreatureManager GetCreatureManager()
    {
        return GetComponent<CreatureController>().creatureMan;
    }

    private void FixedUpdate()
    {
        boundsRadius = GetCreatureManager().boundsRadius;
        centerPoint = GetCreatureManager().boundsCenterPoint;
        var checkBounds = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - centerPoint.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - centerPoint.y), 2));

        if (checkBounds > boundsRadius)
        {
            hungerBar.SetActive(false);
            print("check bound is false: " + gameObject.name);
            return;
        }
        else
        {
            hungerBar.SetActive(true);
        }
        
        if (checkBounds > boundsRadius)
        {
            moveToCenter= true;
        }
        var step = 1f * Time.deltaTime;
        if (moveToCenter)
        {
            if (waitFor == null)
            {
                moveTo = new Vector3(centerPoint.x + Random.Range(-10, 10), centerPoint.y + Random.Range(-10, 10), centerPoint.z);
                waitFor = StartCoroutine(WaitFor(1f));
                rb.velocity = Vector3.zero;
            }
            moving = false;
            StopCoroutine(CreatureIdleMove());
            idleMove = null;
            transform.position = Vector3.Lerp(transform.position, moveTo, step/5);
            
        }
        else if (moveToFood && foodFollower!=null)
        {
            moving= false;
            StopAllCoroutines();
            idleMove = null;
            foodPos = foodFollower.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, foodPos, step);
        }
        else
        {
            if (idleMove == null)
            {
                Debug.Log(name + "start idle move");
                idleMove = StartCoroutine(CreatureIdleMove());
            }
        }
        
    }
    public Vector2 MoveAround()
    {
        Vector2 move = new Vector2(Random.Range(-speed,speed), Random.Range(-speed,speed));
        return move;
    }
    private IEnumerator CreatureIdleMove()
    {
        while (moving)
        {
            rb.velocity = Vector3.zero;
            var move = MoveAround();

            rb.velocity += move;

            facing = rb.rotation * Vector3.forward;

            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
            rb.velocity = Vector3.zero;


            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
        }
    }
    public IEnumerator CreatureEating()
    {
        moving= false;
        StopCoroutine(CreatureIdleMove());
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(3f);

        moving= true;
        idleMove = StartCoroutine(CreatureIdleMove());
    }
    public IEnumerator WaitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moveToCenter= false;
        yield return new WaitForSeconds(waitTime);
        moving = true;
        idleMove = StartCoroutine(CreatureIdleMove());
    }
}
