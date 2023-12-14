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
    
    // Components;
    private Camera cam;
    
    // Move variables
    public bool canMove = false;
    public float moveSpeed = 8f;
    public float smoothTime = 0.1f;
    
    // Zoom variables
    public float zoomSpeed = 20f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    private float targetZoom;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove) return;
        HandleMove();
        HandleZoom();
    }

    void HandleMove()
    {
        float moveX = 0f;
        float moveY = 0f;

        // Get input from WASD keys
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        // Set target position based on input
        targetPosition += new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    
    void HandleZoom()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            targetZoom -= zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            targetZoom += zoomSpeed * Time.deltaTime;
        }

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Smoothly zoom the camera
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime / smoothTime);
    }

    public void ChangeMovementAbility(bool changeTo)
    {
        canMove = changeTo;
    }
}
