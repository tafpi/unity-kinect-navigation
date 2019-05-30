/* https://www.youtube.com/watch?v=n-KX8AeGK7E&ab_channel=AcaciaDeveloper */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;

    private CharacterController characterController;

    [SerializeField] public GameObject travelGesture;
    private Vector3 forwardMovement;
    private float vertInput;
    public float gestureRate;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (travelGesture != null)
        {
            travelGesture.GetComponent<GestureState>().gestureTracked = true;
        }
    }

    private void Update()
    {
        if (travelGesture == null)
        {
            return;
        }
        gestureRate = travelGesture.GetComponent<GestureState>().gestureRate;
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        forwardMovement = transform.forward * gestureRate * movementSpeed;
        characterController.SimpleMove(forwardMovement);
    }

}