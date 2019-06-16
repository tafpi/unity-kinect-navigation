using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureHalfStepForward1 : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumRate = 0.3f;
    public float maximumRate = 0.55f;

    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 spineBase;
    private Vector3 ankleLeftPrev;
    private Vector3 ankleRightPrev;
    public float ankleLeftTravel;
    public float ankleRightTravel;
    private float leftTravelTemp;
    private float rightTravelTemp;

    private Vector3 legLeft;
    private Vector3 legRight;

    public float rate;

    private Vector3 ankleForward;
    private Vector3 ankleMoving;
    private Vector3 feetMiddle;
    public float playerCenterDiff;
    public float direction;
    public bool feetApart;
    public float groundThreshold = 0.1f;
    public float distanceThreshold = 0.1f;
    public bool feetDown;
    private float feetDistance;


    private GestureState state;

    // Start is called before the first frame update
    void Start()
    {
        state = gameObject.GetComponent<GestureState>();
        trackGesture = gameObject.GetComponent<GestureState>().gestureTracked;

        InvokeRepeating("AnkleMoving", 0, 0.5f);
    }

    void AnkleMoving()
    {
        // Debug.Log("test");
        //if (ankleLeftTravel > ankleRightTravel)
        //{
        //    ankleMoving = ankleLeft;
        //    Debug.Log("LEFT");
        //}
        //else
        //{
        //    ankleMoving = ankleRight;
        //    Debug.Log("RIGHT");
        //}
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
                    ankleLeftPrev = ankleLeft;
                    ankleRightPrev = ankleRight;

                    ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);

                    ankleLeftTravel = ankleLeft.z - ankleLeftPrev.z;
                    ankleRightTravel = ankleRight.z - ankleRightPrev.z;

                    if (Mathf.Abs(ankleLeft.y - ankleRight.y) < groundThreshold)
                    {
                        direction = Mathf.Abs(ankleLeftTravel) < Mathf.Abs(ankleRightTravel) ? 1 : -1;
                    } else
                    {
                        direction = 0;
                    }
                }
            }
        }

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
                    gestureRate = 0;
                    

                    leftTravelTemp += ankleLeft.z - ankleLeftPrev.z;
                    rightTravelTemp += ankleRight.z - ankleRightPrev.z;

                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);

                    legLeft = ankleLeft - spineBase;
                    legLeft = new Vector3(0, legLeft.y, legLeft.z);
                    legRight = ankleRight - spineBase;
                    legRight = new Vector3(0, legRight.y, legRight.z);

                    //rate = Mathf.Sin(Vector3.Angle(legLeft, legRight) * Mathf.Deg2Rad);
                    rate = Mathf.Abs(ankleLeft.y - ankleRight.y);

                    ankleForward = (ankleLeft.z < ankleRight.z) ? ankleLeft : ankleRight;

                    

                    //direction = ankleMoving == ankleForward ? 1 : -1;

                    /*
                    if (rate > minimumRate)
                    {
                        // feet apart
                        if (!feetApart)
                        {
                            // first frame feet apart
                        }
                        feetApart = true;
                        if (Mathf.Abs(ankleLeft.y - ankleRight.y) < groundThreshold)
                        {
                            // both feet on the ground: ankles height (y) distance is within threshold
                            rate = Functions.limitValue(minimumRate, maximumRate, rate);
                            gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;
                        }
                        else
                        {
                            // a foot is off the ground. ankles height (y) distance is out of threshold
                        }
                    }
                    else
                    {
                        // feet close to eachother
                        feetApart = false;

                        feetMiddle = Vector3.Lerp(ankleLeft, ankleRight, 0.5f);
                    }
                    */

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }
            }
        }

    }

}
