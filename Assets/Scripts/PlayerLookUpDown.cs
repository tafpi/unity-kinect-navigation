using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookUpDown : MonoBehaviour
{
    public float defaultRotationSpeed = 10;
    private float rotationSpeed;
    private CharacterController characterController;


    public GameObject rotateGesture;
    public float rotate;
    public float gestureRate;

    private float xAxisClamp;
    private Vector3 eulerRotation;

    public float euler;
    public bool outOfRange;

    private GameObject player;
    private PlayerManager playerManager;
    private GestureState gestureState;


    private void Awake()
    {
        //LockCursor();
        xAxisClamp = 0.0f;
        eulerRotation = Vector3.zero;
        player = transform.parent.gameObject;
        playerManager = GetComponentInParent<PlayerManager>();

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
        {
            return;
        }
        if(!playerManager.travelling)
        {
            gestureRate = gestureState.gestureRate;
            CameraRotation();
        } else
        {
            ResetCamera();
        }
    }

    private void CameraRotation()
    {
        if (gestureRate != 0)
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
            transform.Rotate(Vector3.left * rotate);
            playerManager.rotatingX = true;
        }
        else
        {
            playerManager.rotatingX = false;
        }
    }

    private void ClampXAxisRotationToValue(float value)
    {
        eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    private void ResetCamera()
    {
        float step = rotationSpeed * Time.deltaTime;        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, step);
    }
}