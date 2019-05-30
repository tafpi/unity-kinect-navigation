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
        LockCursor();
        //xAxisClamp = 0.0f;
    }


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        //float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        //xAxisClamp += mouseY;

        //if (xAxisClamp > 90.0f)
        //{
        //    xAxisClamp = 90.0f;
        //    mouseY = 0.0f;
        //    ClampXAxisRotationToValue(270.0f);
        //}
        //else if (xAxisClamp < -90.0f)
        //{
        //    xAxisClamp = -90.0f;
        //    mouseY = 0.0f;
        //    ClampXAxisRotationToValue(90.0f);
        //}

        //transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * gestureRate * rotationSpeed);
    }

    //private void ClampXAxisRotationToValue(float value)
    //{
    //    Vector3 eulerRotation = transform.eulerAngles;
    //    eulerRotation.x = value;
    //    transform.eulerAngles = eulerRotation;
    //}
}