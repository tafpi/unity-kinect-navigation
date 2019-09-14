﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

// Speed Threshold for returning hand to rest position after controlling the view

public class GesturePointUpDown03 : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public GameObject player;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumAngle = 30;
    public float maximumAngle = 65;

    private Vector3 wristLeft;
    private Vector3 wristRight;
    private Vector3 arm;

    private float wristShoulderDistance;
    private float armLength;
    private float armTension;
    public float angle;
    public float zeroAngleDiff = 10;    // angle diff to align perceived and actual horizontal arm position (perceived is lower)

    public float armSpeed;
    public float armSpeedMax = 0.2f;
    public Vector3 previousHandPosition;

    public float rate;

    private GestureState state;

    // Start is called before the first frame update
    void Start()
    {
        state = gameObject.GetComponent<GestureState>();
        trackGesture = gameObject.GetComponent<GestureState>().gestureTracked;
    }

    // Update is called once per frame
    void Update()
    {

        if (trackGesture)
        {
            Debug.Log("point forward tracked");
            if (_bodySourceManager == null)
            {
                return;
            }

            _bodyManager = _bodySourceManager.GetComponent<BodySourceManager>();
            if (_bodyManager == null)
            {
                return;
            }

            Body[] data = _bodyManager.GetData();
            if (data == null)
            {
                return;
            }

            // get the first tracked body...
            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }

                if (body.IsTracked)
                {
                    Vector3 spineShoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);
                    Hand hand = ActiveHand(body, spineShoulder);
                    Vector3 wrist = hand.WristPoint;
                    Vector3 elbow = hand.ElbowPoint;
                    
                    armTension = (spineShoulder - wrist).magnitude / ((spineShoulder - elbow).magnitude + (elbow - wrist).magnitude);
                    arm = wrist - spineShoulder;
                    arm.x = 0;

                    angle = Vector3.Angle(arm, Vector3.down) - 90 + zeroAngleDiff;

                    armSpeed = Vector3.Distance(hand.WristPoint, previousHandPosition) / Time.deltaTime;
                    previousHandPosition = hand.WristPoint;
                    
                    if (angle < -maximumAngle)
                    {
                        gestureRate = 0;
                    }
                    else
                    {
                        rate = angle * armTension;
                        rate = Mathf.Sign(angle) * Functions.limitValue(minimumAngle, maximumAngle, Mathf.Abs(rate));
                        gestureRate = armSpeed < armSpeedMax ? -Mathf.Sign(angle) * Mathf.Pow((Mathf.Abs(rate) - minimumAngle) / (maximumAngle - minimumAngle), slope) : 0;
                    }

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }

            }

        }

    }

    private Hand ActiveHand(Body body, Vector3 spineShoulder)
    {
        CameraSpacePoint wristLeftPoint = body.Joints[JointType.WristLeft].Position;
        CameraSpacePoint wristRightPoint = body.Joints[JointType.WristRight].Position;

        wristLeft = Functions.unityVector3(wristLeftPoint);
        wristRight = Functions.unityVector3(wristRightPoint);

        // which wrist is further from the body
        Hand hand = new Hand(   wristRightPoint,
                                body.Joints[JointType.ElbowRight].Position,
                                Vector3.zero,
                                Vector3.zero,
                                body.HandRightState);

        if (Mathf.Abs(wristLeft.z - spineShoulder.z) > Mathf.Abs(wristRight.z - spineShoulder.z))
        {
            hand.Wrist = wristLeftPoint;
            hand.Elbow = body.Joints[JointType.ElbowLeft].Position;
            hand.Fist = body.HandLeftState;
        }

        return hand.SetHandPoints();
    }

}
