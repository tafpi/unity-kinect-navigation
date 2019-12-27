using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookLeftRight : MonoBehaviour
{
    public float defaultRotationSpeed = 2;
    private float rotationSpeed = 1;

    public GameObject rotateGesture;
    private float rotation;
    public float gestureRate;

    private CharacterController characterController;
    private PlayerManager playerManager;
    private GestureState gestureState;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        if (rotateGesture != null)
        {
            gestureState = rotateGesture.GetComponent<GestureState>();
            gestureState.gestureTracked = true;
            rotationSpeed = defaultRotationSpeed;
            if (gestureState.rotationSpeed != 0)
                rotationSpeed = gestureState.rotationSpeed;
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