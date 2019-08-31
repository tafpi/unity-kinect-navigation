using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookLeftRight : MonoBehaviour
{
    private float defaultRotationSpeed = 1;
    private float rotationSpeed = 1;
    private CharacterController characterController;

    public GameObject playerBody;

    public GameObject rotateGesture;
    private float rotation;
    public float gestureRate;

    private void Awake()
    {
        //LockCursor();
        if (rotateGesture != null)
        {
            rotateGesture.GetComponent<GestureState>().gestureTracked = true;
            if (rotateGesture.GetComponent<GestureState>().rotationSpeed != 0)
            {
                rotationSpeed = rotateGesture.GetComponent<GestureState>().rotationSpeed;
            }
            else
            {
                rotationSpeed = defaultRotationSpeed;
            }
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
        playerBody.transform.Rotate(Vector3.up * gestureRate * rotationSpeed);
        //playerBody.Rotate(Vector3.up * gestureRate * rotationSpeed);
    }
}