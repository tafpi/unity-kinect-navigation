using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureHalfStepSideways : MonoBehaviour
{
    // NOTES: 

    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;

    public float gestureRate;

    public float slope = 2.5f;
    public float minimumRate = 0.3f;
    public float maximumRate = 0.5f;

    private Vector3 spineBase;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 ankleLeftPrev;
    private Vector3 ankleRightPrev;

    private Vector3 legLeft;
    private Vector3 legRight;

    private float ankleMovingAway, ankleMovingClose;  // -1 is left, 1 is right. 0 is unknown
    public float ankleLeftTravel;
    public float ankleRightTravel;


    public float rate;
    public float ratePeak;
    public float direction;
    public float directionPrev;
    public bool feetApart;

    public float groundThreshold = 0.1f;
    public float distanceThreshold = 0.1f;

    private GestureState state;

    // Start is called before the first frame update
    void Start()
    {
        feetApart = false;
        direction = 0;
        state = gameObject.GetComponent<GestureState>();
        trackGesture = gameObject.GetComponent<GestureState>().gestureTracked;

        InvokeRepeating("ResetAnkleTravel", 0, 0.5f);
    }

    void ResetAnkleTravel()
    {
        ankleLeftTravel = 0;
        ankleRightTravel = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (trackGesture)
        {
            Debug.Log("step forward tracked");
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
                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);

                    ankleLeftTravel += ankleLeft.z - ankleLeftPrev.z;
                    ankleRightTravel += ankleRight.z - ankleRightPrev.z;

                    legLeft = ankleLeft - spineBase;
                    legLeft = new Vector3(legLeft.x, legLeft.y, 0);
                    legRight = ankleRight - spineBase;
                    legRight = new Vector3(legRight.x, legRight.y, 0);

                    rate = Mathf.Sin(Vector3.Angle(legLeft, legRight) * Mathf.Deg2Rad);

                    if (rate > minimumRate)
                    {
                        // feet apart
                        if (!feetApart)
                        {
                            // feet first frame apart
                            feetApart = true;

                            direction = Mathf.Abs(ankleLeftTravel) > Mathf.Abs(ankleRightTravel) ?
                                Mathf.Sign(ankleLeftTravel) : Mathf.Sign(ankleRightTravel);
                        }
                        //gestureRate = 0;
                    }
                    else
                    {
                        // feet close to eachother
                        feetApart = false;
                        direction = 0;
                    }

                    rate = Functions.limitValue(minimumRate, maximumRate, rate);
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }
            }
        }

    }

}
