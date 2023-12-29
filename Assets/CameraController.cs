using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] cameraPositions;

    public static CameraController Instance;
    private bool needToMove = false;
    private int CameraPositionIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCameraPos(int index)
    {
        CameraPositionIndex = index;
        needToMove = true;
    }
    private void Update()
    {
        if (!needToMove) return;

        transform.position = Vector3.Lerp(transform.position, cameraPositions[CameraPositionIndex].transform.position, Time.deltaTime * 1.5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraPositions[CameraPositionIndex].transform.rotation, Time.deltaTime * 1.5f);

        if(transform.position == cameraPositions[CameraPositionIndex].transform.position && transform.rotation == cameraPositions[CameraPositionIndex].transform.rotation)
        {
            needToMove = false;
        }

    }
}
