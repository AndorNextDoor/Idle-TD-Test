using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem instance;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool useDrag = false;
    [SerializeField] private float fieldOfViewMax = 50;
    [SerializeField] private float fieldOfViewMin = 10;
    private float targetFieldOfView = 100f;

    [SerializeField] private float dragSpeed;

    private float rotationStartAngle = 0f;
    private float previouseDistance = 0f;
    private float distance = 0f;

    private bool isZooming = false;

    public bool isDisabled;

    [SerializeField] private Transform rotationPivot;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isDisabled) return;
        HandleCameraRotation();
        HandleCameraZoom();
    }

    //Later if we need to move the camera pos in different projects
    private void HandleCameraMovement()
    {
        if (isZooming) return;
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Use deltaPosition to get the change in touch position
            inputDir.x = touch.deltaPosition.x * moveSpeed * Time.deltaTime;
            inputDir.z = touch.deltaPosition.y * moveSpeed * Time.deltaTime;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir;
    }

    private void HandleCameraRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the rotation angle based on the touch delta
                float rotationAngle = touch.deltaPosition.x * rotateSpeed * Time.deltaTime;

                // Rotate the camera around the pivot point
                transform.RotateAround(rotationPivot.position, Vector3.up, rotationAngle);
            }
        }
    }

    private void HandleCameraZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            distance = Vector2.Distance(touch1.position, touch2.position);

            if (distance > previouseDistance)
            {
                // Zoom in
                targetFieldOfView -= 5f;
                isZooming = true;
            }
            else if (distance < previouseDistance)
            {
                // Zoom out
                targetFieldOfView += 5f;
                isZooming = true;
            }

            previouseDistance = distance;

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);
            cinemachineVirtualCamera.m_Lens.FieldOfView = targetFieldOfView;
        }
        else
        {
            isZooming = false;
        }
    }
}