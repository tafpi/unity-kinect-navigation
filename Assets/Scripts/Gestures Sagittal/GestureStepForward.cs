using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureStepForward : MonoBehaviour
{
    // NOTES: 

    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;

    public float gestureRate;

    [Range(1f, 2f)] public float slope = 1.5f;
    [Range(0f, 1f)] public float minimumRate = 0.2f;
    [Range(0f, 1f)] public float maximumRate = 0.5f;

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
        ankleMovingAway = 0;
        ankleMovingClose = 0;
        state = gameObject.GetComponent<GestureState>();

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
                legLeft = new Vector3(0, legLeft.y, legLeft.z);
                legRight = ankleRight - spineBase;
                legRight = new Vector3(0, legRight.y, legRight.z);

                rate = Mathf.Sin(Vector3.Angle(legLeft, legRight) * Mathf.Deg2Rad);
                ratePeak = rate > ratePeak ? rate : ratePeak;

                if (rate > minimumRate)
                {
                    // feet apart
                    if (!feetApart)
                    {
                        // apart first frame
                        feetApart = true;

                        gestureRate = 0;

                        // left ankle is -1, right is 1
                        ankleMovingAway = Mathf.Abs(ankleLeftTravel) > Mathf.Abs(ankleRightTravel) ? -1 : 1;
                    }
                }
                else
                {
                    // feet close
                    if (feetApart)
                    {
                        // close first frame
                        feetApart = false;

                        directionPrev = direction;

                        ankleMovingClose = Mathf.Abs(ankleLeftTravel) > Mathf.Abs(ankleRightTravel) ? -1 : 1;

                        if (ankleMovingClose != ankleMovingAway)
                        {
                            // moving opposite last moving foot
                            direction = ankleMovingClose == -1 ? -Mathf.Sign(ankleLeftTravel) : -Mathf.Sign(ankleRightTravel);
                            
                            if (direction == -directionPrev)
                            {
                                direction = 0;
                                gestureRate = 0;
                            }
                            else
                            {
                                rate = Functions.limitValue(minimumRate, maximumRate, ratePeak);
                                gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;
                            }

                        }

                    }
                    ratePeak = 0;
                }


                if (state != null) state.gestureRate = gestureRate;

                break;
            }
        }

    }

    private void OnValidate()
    {
        if (minimumRate > maximumRate) maximumRate = minimumRate;
    }

}
