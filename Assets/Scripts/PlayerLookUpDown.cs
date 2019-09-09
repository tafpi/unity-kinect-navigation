using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookUpDown : MonoBehaviour
{
    public float defaultRotationSpeed = 10;
    private float rotationSpeed;
    private CharacterController characterController;

    public GameObject playerBody;

    public GameObject rotateGesture;
    public float rotate;
    public float gestureRate;

    private float xAxisClamp;

    public float euler;
    public bool outOfRange;

    private void Awake()
    {
        //LockCursor();
        xAxisClamp = 0.0f;

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
        if(playerBody.GetComponent<PlayerMove>().gestureRate == 0)
        {
            gestureRate = rotateGesture.GetComponent<GestureState>().gestureRate;
            CameraRotation();
        } else
        {
            ResetCamera();
        }
    }

    private void CameraRotation()
    {
        rotate = -gestureRate * rotationSpeed * Time.deltaTime;
        xAxisClamp += rotate;
        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            rotate = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            rotate = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }
        transform.Rotate(Vector3.left*rotate);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    private void ResetCamera()
    {
        float step = rotationSpeed * Time.deltaTime;        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerBody.transform.rotation, step);
    }
}