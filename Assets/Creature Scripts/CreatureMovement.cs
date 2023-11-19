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


    private CreatureController myController;

    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        myController = GetComponent<CreatureController>();
    }
    private void Start()
    {
        StartCoroutine(CreatureIdleMove());
    }
    private void Update()
    {

    }
    public void MoveToFood(Vector3 foodPos)
    {
        float step = 0.5f;
        transform.position = Vector3.Lerp(transform.position, foodPos, step);
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
    public IEnumerator CreatureEating(Vector3 foodPos)
    {
        StopCoroutine(CreatureIdleMove());
        while(transform.position != foodPos)
        {
            MoveToFood(foodPos);
        }
        myController.hunger.HungerGoUp(10f);
        yield return new WaitForSeconds(7f);

        StartCoroutine(CreatureIdleMove());
    }
}
