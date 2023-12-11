using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager instance;
    public static CameraManager i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CameraManager>();
            }
            return instance;
        }
    }
    
    public bool canMove = false;
    public float moveSpeed = 8f;
    public float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove) return;
        float moveX = 0f;
        float moveY = 0f;

        // Get input from WASD keys
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }

        // Set target position based on input
        targetPosition += new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void ChangeMovementAbility(bool changeTo)
    {
        canMove = changeTo;
    }
}
