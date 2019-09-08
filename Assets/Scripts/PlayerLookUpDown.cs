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
    //private float rotation;
    public float gestureRate;

    private float xAxisClamp;

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
        Debug.Log(transform.eulerAngles.x);

        if ((transform.eulerAngles.x < 90.0f) || (transform.eulerAngles.x > 270.0f))
        {
            transform.Rotate(Vector3.right * gestureRate * rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (transform.eulerAngles.x < 180.0f)
            {
                transform.Rotate(Vector3.left, 90);
            }
            if (transform.eulerAngles.x > 180.0f)
            {
                transform.Rotate(Vector3.left, 270);
            }
        }


    }

    private void ClampXAxisRotationToValue(float value)
    {
        //Vector3 eulerRotation = transform.eulerAngles;
        //eulerRotation.x = value;
        //transform.eulerAngles = eulerRotation;

        //transform.Rotate(Vector3.right * gestureRate * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, value);
    }

    private void ResetCamera()
    {
        float step = rotationSpeed * Time.deltaTime;        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerBody.transform.rotation, step);
    }
}