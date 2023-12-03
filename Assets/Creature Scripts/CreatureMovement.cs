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
    private Coroutine idleMove;
    public Vector3 startPos;

    private CreatureController myController;

    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        myController = GetComponent<CreatureController>();
    }
    private void Start()
    {
        idleMove = StartCoroutine(CreatureIdleMove());
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        var step = 1f * Time.deltaTime;
        if(moveToFood && foodFollower!=null)
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
            rb.velocity = MoveAround();
            

            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
            rb.velocity = Vector3.zero;
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -boundsX, boundsX), Mathf.Clamp(transform.position.y, -boundsY, boundsY), transform.position.z);

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
        StartCoroutine(CreatureIdleMove());
    }
}
