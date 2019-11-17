using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

// Speed Threshold for returning hand to rest position after controlling the view

public class GesturePointUpDown : MonoBehaviour
{
    public GameObject bodySourceManager;
    private BodySourceManager bodyManager;
    private bool trackGesture = false;
    
    public float gestureRate;

    [Range(1f, 2f)] public float slope = 1.2f;
    [Range(0f, 90f)] public float minimumAngle = 12;
    [Range(0f, 90f)] public float maximumAngle = 45;
    public float zeroAngleDiff = 10;    // angle diff to align perceived and actual horizontal arm position (perceived is lower)


    private float wristShoulderDistance;
    private float armLength;
    private float armTension;
    public float angle;

    public float armSpeed;
    public float armSpeedMax = 0.9f;
    public Vector3 previousHandPosition;

    public float rate;

    private GestureState state;

    
    private Vector3 arm;
    private CameraSpacePoint wristLeft;
    private CameraSpacePoint wristRight;
    private CameraSpacePoint spineShoulder;
    private CameraSpacePoint wrist;
    private CameraSpacePoint elbow;

    void Start()
    {
        state = gameObject.GetComponent<GestureState>();
        trackGesture = gameObject.GetComponent<GestureState>().gestureTracked;
    }
    
    void Update()
    {

        if (trackGesture)
        {
            //Debug.Log("point forward tracked");
            if (bodySourceManager == null)
            {
                return;
            }

            bodyManager = bodySourceManager.GetComponent<BodySourceManager>();
            if (bodyManager == null)
            {
                return;
            }

            Body[] data = bodyManager.GetData();
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
                    spineShoulder = body.Joints[JointType.SpineShoulder].Position;
                    wristLeft = body.Joints[JointType.WristLeft].Position;
                    wristRight = body.Joints[JointType.WristRight].Position;

                    // which wrist is further from the body
                    if (Mathf.Abs(wristLeft.Z - spineShoulder.Z) > Mathf.Abs(wristRight.Z - spineShoulder.Z))
                    {
                        wrist = wristLeft;
                        elbow = body.Joints[JointType.ElbowLeft].Position;
                    }
                    else
                    {
                        wrist = wristRight;
                        elbow = body.Joints[JointType.ElbowRight].Position;
                    }

                    Vector3 spineShoulderV = Functions.unityVector3(spineShoulder);
                    Vector3 wristV = Functions.unityVector3(wrist);
                    Vector3 elbowV = Functions.unityVector3(elbow);

                    armTension = (spineShoulderV - wristV).magnitude / ((spineShoulderV - elbowV).magnitude + (elbowV - wristV).magnitude);
                    arm = wristV - spineShoulderV;
                    arm.x = 0;

                    //angle = Vector3.Angle(arm, Vector3.forward);
                    angle = Vector3.Angle(arm, Vector3.down) - 90 + zeroAngleDiff;

                    armSpeed = Vector3.Distance(wristV, previousHandPosition) / Time.deltaTime;
                    previousHandPosition = wristV;

                    //if (angle < -maximumAngle)
                    //{
                    //    gestureRate = 0;
                    //}
                    //else
                    //{
                    //    rate = angle * armTension;
                    //    rate = Mathf.Sign(angle) * Functions.limitValue(minimumAngle, maximumAngle, Mathf.Abs(rate));
                    //    gestureRate = armSpeed < armSpeedMax ? -Mathf.Sign(angle) * Mathf.Pow((Mathf.Abs(rate) - minimumAngle) / (maximumAngle - minimumAngle), slope) : 0;
                    //}

                    //rate = angle * armTension;
                    rate = Functions.limitValue(minimumAngle, maximumAngle, Mathf.Abs(angle * armTension));
                    gestureRate = armSpeed < armSpeedMax ? -Mathf.Sign(angle) * Mathf.Pow((Mathf.Abs(rate) - minimumAngle) / (maximumAngle - minimumAngle), slope) : 0;
                    if (angle < -maximumAngle)
                        gestureRate = 0;

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }

            }

        }

    }

    //private Hand ActiveHand(Body body, Vector3 spineShoulder)
    //{
    //    //CameraSpacePoint wristLeftPoint = body.Joints[JointType.WristLeft].Position;
    //    //CameraSpacePoint wristRightPoint = body.Joints[JointType.WristRight].Position;

    //    wristLeft = Functions.unityVector3(body.Joints[JointType.WristLeft].Position);
    //    wristRight = Functions.unityVector3(body.Joints[JointType.WristRight].Position);

    //    // which wrist is further from the body

    //    if (Mathf.Abs(wristLeft.z - spineShoulder.z) > Mathf.Abs(wristRight.z - spineShoulder.z))
    //    {
    //        hand = new Hand(
    //            body.Joints[JointType.WristLeft].Position,
    //            body.Joints[JointType.ElbowLeft].Position,
    //            wristLeft,
    //            Functions.unityVector3(body.Joints[JointType.ElbowLeft].Position)
    //            );
    //    }
    //    else
    //    {
    //        hand = new Hand(
    //            body.Joints[JointType.WristRight].Position,
    //            body.Joints[JointType.ElbowRight].Position,
    //            wristRight,
    //            Functions.unityVector3(body.Joints[JointType.ElbowRight].Position)
    //            );
    //    }

    //    return hand;
    //}

    private void OnValidate()
    {
        if (minimumAngle > maximumAngle) maximumAngle = minimumAngle;
    }

}
