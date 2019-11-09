/* https://www.youtube.com/watch?v=n-KX8AeGK7E&ab_channel=AcaciaDeveloper */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject travelGesture;
    public float gestureRate;
    public float movementSpeed = 10;
    [Range(10, 30)] public int interval = 20;

    private CharacterController characterController;
    private float vertInput;
    private Vector3 forwardMovement;

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
            playerManager.travelling = false;
            return;
        }
        playerManager.travelling = true;
        gestureRate = travelGesture.GetComponent<GestureState>().gestureRate;
        forwardMovement = transform.forward * gestureRate * movementSpeed;
        characterController.SimpleMove(forwardMovement);
        //PlayerMovement();
    }

    //private void PlayerMovement()
    //{
    //}

    public void SetToCannotMove()
    {
        playerManager.canMove = false;
    }

}