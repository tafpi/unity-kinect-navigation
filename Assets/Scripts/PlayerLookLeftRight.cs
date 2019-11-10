using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookLeftRight : MonoBehaviour
{
    private float defaultRotationSpeed = 1;
    private float rotationSpeed = 1;
    //public GameObject playerBody;

    public GameObject rotateGesture;
    private float rotation;
    public float gestureRate;

    private CharacterController characterController;
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
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
            return;
        gestureRate = rotateGesture.GetComponent<GestureState>().gestureRate;
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (gestureRate != 0)
        {
            transform.Rotate(Vector3.up * gestureRate * rotationSpeed);
            playerManager.rotatingY = true;
        }
        else
        {
            playerManager.rotatingY = false;
        }
    }
}