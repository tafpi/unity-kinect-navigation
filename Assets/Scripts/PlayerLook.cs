using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1;

    private CharacterController characterController;

    [SerializeField] private Transform playerBody;

    [SerializeField] public GameObject rotateGesture;
    private float rotation;
    public float gestureRate;

    private void Awake()
    {
        //LockCursor();
        if(rotateGesture != null)
        {
            rotateGesture.GetComponent<GestureState>().gestureTracked = true;
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (rotateGesture == null)
        {
            return;
        }
        gestureRate = rotateGesture.GetComponent<GestureState>().gestureRate;
        CameraRotation();
    }

    private void CameraRotation()
    {
        //transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * gestureRate * rotationSpeed);
    }
}