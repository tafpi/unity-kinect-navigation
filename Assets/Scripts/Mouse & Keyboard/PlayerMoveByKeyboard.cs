/* https://www.youtube.com/watch?v=n-KX8AeGK7E&ab_channel=AcaciaDeveloper */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveByKeyboard : MonoBehaviour
{
    public string horizontalInputName = null;
    public string verticalInputName = null;
    public float movementSpeed = 10;
    
    private PlayerManager playerManager;
    private CharacterController charController;
    private Vector3 forwardMovement;
    private Vector3 rightMovement;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (playerManager.canMove)
        {
            float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
            float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

            if (horizInput == 0 && vertInput == 0)
            {
                playerManager.travelling = false;
                return;
            }
            playerManager.travelling = true;
            if (vertInput < 0)
                playerManager.travellingBackwards = true;
            forwardMovement = transform.forward * vertInput;
            rightMovement = transform.right * horizInput;
            charController.SimpleMove(forwardMovement + rightMovement);
        }
    }

    public void SetToCannotMove()
    {
        playerManager.canMove = false;
    }

}