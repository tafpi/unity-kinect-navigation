/* https://www.youtube.com/watch?v=n-KX8AeGK7E&ab_channel=AcaciaDeveloper */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveByKeyboard : MonoBehaviour
{
    public string horizontalInputName = null;
    public string verticalInputName = null;
    public float movementSpeed = 10;
    public bool canMove = true;

    private CharacterController charController;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (canMove)
        {
            float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
            float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horizInput;

            charController.SimpleMove(forwardMovement + rightMovement);
        }
    }

    public void SetToCannotMove()
    {
        Debug.Log("cannot");
        canMove = false;
    }

}