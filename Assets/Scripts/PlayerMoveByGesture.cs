/* https://www.youtube.com/watch?v=n-KX8AeGK7E&ab_channel=AcaciaDeveloper */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveByGesture : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private CharacterController characterController;

    [SerializeField] public GameObject _travelGesture;
    private float _gestureRate;
    private Vector3 forwardMovement;
    private float vertInput;

    private bool gestureRaiseHandTracked;
    private bool gestureBendForwardTracked;
    private bool gestureBendKneeForwardTracked;
    private bool gestureHalfStepForwardTracked;
    private bool gesturePointForwardTracked;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (_travelGesture != null)
        {
            if (_travelGesture.GetComponent<GestureRaiseHand>()) {
                gestureRaiseHandTracked = _travelGesture.GetComponent<GestureRaiseHand>().trackGesture;
            }
            if (_travelGesture.GetComponent<GestureBendForward>())            {
                gestureBendForwardTracked = _travelGesture.GetComponent<GestureBendForward>().trackGesture;
            }
            if (_travelGesture.GetComponent<GestureBendKneeForward>())
            {
                gestureBendKneeForwardTracked = _travelGesture.GetComponent<GestureBendKneeForward>().trackGesture;
            }
            if (_travelGesture.GetComponent<GestureHalfStepForward>())
            {
                gestureHalfStepForwardTracked = _travelGesture.GetComponent<GestureHalfStepForward>().trackGesture;
            }
            if (_travelGesture.GetComponent<GesturePointForward>())
            {
                gesturePointForwardTracked = _travelGesture.GetComponent<GesturePointForward>().trackGesture;
            }
        }
    }

    private void Update()
    {
        if (_travelGesture == null)
        {
            return;
        }
        
        if (gestureRaiseHandTracked)
        {
            _gestureRate = _travelGesture.GetComponent<GestureRaiseHand>().gestureRate;
        }
        if (gestureBendForwardTracked)
        {
            _gestureRate = _travelGesture.GetComponent<GestureBendForward>().gestureRate;
        }
        if (gestureBendKneeForwardTracked)
        {
            _gestureRate = _travelGesture.GetComponent<GestureBendKneeForward>().gestureRate;
        }
        if (gestureHalfStepForwardTracked)
        {
            _gestureRate = _travelGesture.GetComponent<GestureHalfStepForward>().gestureRate;
        }
        if (gesturePointForwardTracked)
        {
            _gestureRate = _travelGesture.GetComponent<GesturePointForward>().gestureRate;
        }

        PlayerMovement();
    }

    private void PlayerMovement()
    {
        //float vertInput = _gestureRate * movementSpeed;
        //Vector3 forwardMovement = transform.forward * vertInput;

        forwardMovement = transform.forward * _gestureRate * movementSpeed;
        characterController.SimpleMove(forwardMovement);

    }

}