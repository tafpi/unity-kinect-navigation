﻿/* https://www.youtube.com/watch?v=n-KX8AeGK7E&ab_channel=AcaciaDeveloper */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject travelGesture;
    public float gestureRate;
    public float movementSpeed = 10;
    [Range(10, 30)] public int interval = 20;

    private PlayerManager playerManager;
    private CharacterController characterController;
    private GestureState gestureState;
    private float vertInput;
    private Vector3 forwardMovement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerManager = GetComponent<PlayerManager>();
        gestureState = travelGesture.GetComponent<GestureState>();
        if (travelGesture != null)
        {
            gestureState.gestureTracked = true;
        }
    }

    private void Update()
    {
        if (playerManager.canMove)
        {
            if (travelGesture == null)
                return;
            gestureRate = gestureState.gestureRate;
            if (gestureRate != 0)
            {
                forwardMovement = transform.forward * gestureRate * movementSpeed;
                characterController.SimpleMove(forwardMovement);
                playerManager.travelling = true;
                playerManager.travellingBackwards = false;
                if (gestureRate < 0)
                    playerManager.travellingBackwards = true;
            }
            else
            {
                playerManager.travelling = false;
            }
        }
    }

    public void SetToCannotMove()
    {
        playerManager.canMove = false;
    }

}