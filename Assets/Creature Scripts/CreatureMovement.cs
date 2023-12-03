using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTimeMin = 2f, waitTimeMax = 5f;
    [SerializeField] private float boundsY=5, boundsX=9;

    private Rigidbody2D rb;
    private bool moving = true;
    public Vector3 foodPos;
    public bool moveToFood;
    private Coroutine idleMove;

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
    private void FixedUpdate()
    {
        if(moveToFood)
        {
            StopAllCoroutines();
            idleMove = null;
            transform.position = Vector3.Lerp(transform.position, foodPos, 0.5f*Time.deltaTime);
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

        yield return new WaitForSeconds(6f);

        moving= true;
        StartCoroutine(CreatureIdleMove());
    }
}
