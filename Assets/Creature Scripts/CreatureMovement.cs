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

    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(CreatureMoving());
    }
    private void Update()
    {

    }
    public Vector2 MoveAround()
    {
        Vector2 move = new Vector2(Random.Range(-speed,speed), Random.Range(-speed,speed));
        return move;
    }
    private IEnumerator CreatureMoving()
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
}