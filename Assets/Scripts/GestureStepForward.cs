﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureStepForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;

    public float slope = 1.2f;
    //public string footForward;
    //public Vector3 playerCenter = new Vector3(0,0,0);
    //public float freeMovementRange = 0.02f;
    public float playerCenterDiff;

    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 hipLeft;
    private Vector3 hipRight;
    private Vector3 kneeLeft;
    private Vector3 kneeRight;
    private Vector3 ankleLeftPrev;
    private Vector3 ankleRightPrev;

    private Vector3 ankleForward;
    private Vector3 ankleMoving;
    private JointType jointMovingClose;
    private JointType jointMovingAway;
    private Vector3 feetMiddle;
    public float direction;
    private float directionPrev;
    public bool feetApart;
    
    public bool ? centered = null;

    public float groundThreshold = 0.1f;
    public float distanceThreshold = 0.1f;
    private float legLength;
    private float feetDistance;
    public float ratio;
    public float ratioPeak;
    public float minimumRatio = 0.3f;
    public float maximumRatio = 0.55f;

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
            Debug.Log("half step forward tracked");
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

                    //gestureRate = 0;
                    ankleLeftPrev = ankleLeft;
                    ankleRightPrev = ankleRight;
                    directionPrev = direction;
                    ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);

                    // depth (z) feet distance greater than horizontal (x) feet distance
                    kneeLeft = Functions.unityVector3(body.Joints[JointType.KneeLeft].Position);
                    kneeRight = Functions.unityVector3(body.Joints[JointType.KneeRight].Position);
                    hipLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    hipRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position);

                    ankleForward = (ankleLeft.z < ankleRight.z) ? ankleLeft : ankleRight;
                    legLength = (ankleLeft - kneeLeft).magnitude + (hipLeft - kneeLeft).magnitude;
                    feetDistance = (ankleRight - ankleLeft).magnitude;
                    ratio = feetDistance / legLength;

                    if (ratio > minimumRatio)
                    {
                        // feet apart
                        if (Mathf.Abs(ankleLeft.z - ankleRight.z) > Mathf.Abs(ankleLeft.x - ankleRight.x))
                        {
                            // foot forwards/backwards
                            if (!feetApart)
                            {
                                // first frame feet apart
                                Debug.Log("// first frame feet apart");

                                ratioPeak = 0;

                                //// which foot traveled more since last frame?
                                if (Mathf.Abs(ankleLeft.z - ankleLeftPrev.z) > Mathf.Abs(ankleRight.z - ankleRightPrev.z))
                                {
                                    jointMovingAway = JointType.AnkleLeft;
                                    //direction = Mathf.Sign(ankleLeftPrev.z - ankleLeft.z);
                                }
                                else
                                {
                                    jointMovingAway = JointType.AnkleRight;
                                    //direction = Mathf.Sign(ankleRightPrev.z - ankleRight.z);
                                }

                                //Debug.Log(jointMovingAway);
                            }
                            if(ratio > ratioPeak)
                            {
                                ratioPeak = ratio;
                            }
                        }
                        feetApart = true;
                        //direction = 0;
                        gestureRate = 0;
                    }
                    else
                    {
                        //if (!centered.HasValue)
                        //{
                        //    centered = true;
                        //    direction = 0;
                        //}

                        // feet close to eachother
                        if (Mathf.Abs(ankleLeft.y - ankleRight.y) < groundThreshold)
                        {
                            // both feet on the ground: ankles height (y) distance is within threshold
                            if (feetApart)
                            {
                                // first frame feet close again
                                Debug.Log("first frame feet close again");
                                

                                // which foot traveled more since last frame?
                                if (Mathf.Abs(ankleLeft.z - ankleLeftPrev.z) > Mathf.Abs(ankleRight.z - ankleRightPrev.z))
                                {
                                    jointMovingClose = JointType.AnkleLeft;
                                    direction = Mathf.Sign(ankleLeftPrev.z - ankleLeft.z);
                                }
                                else
                                {
                                    jointMovingClose = JointType.AnkleRight;
                                    direction = Mathf.Sign(ankleRightPrev.z - ankleRight.z);
                                }
                                
                                if ( (directionPrev != 0) && (direction != directionPrev) )
                                {
                                    centered = true;
                                    direction = 0;
                                }
                                ratioPeak = Functions.limitValue(minimumRatio, maximumRatio, ratioPeak);
                                gestureRate = Mathf.Pow((ratioPeak - minimumRatio) / (maximumRatio - minimumRatio), slope) * direction;
                                
                            }
                            
                            feetApart = false;
                        }
                    }

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }
            }
        }

    }

}
