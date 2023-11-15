using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTimeMin = 2f, waitTimeMax = 5f;

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
    public void MoveAround()
    {
        Vector2 move = new Vector2(Random.Range(-speed,speed), Random.Range(-speed,speed));
        rb.velocity = move;
    }
    private IEnumerator CreatureMoving()
    {
        while (moving)
        {
            MoveAround();
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
        }
    }
}
