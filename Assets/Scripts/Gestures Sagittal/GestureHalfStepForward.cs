using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureHalfStepForward : MonoBehaviour
{

    //  NOTES: feet don't have to be both on the ground

    public GameObject bodySourceManager;
    private BodySourceManager bodyManager;
    
    public float gestureRate;

    [Range(1f, 2f)] public float slope = 2f;
    [Range(0f, 1f)] public float minimumRate = 0.2f;
    [Range(0f, 1f)] public float maximumRate = 0.4f;

    private Vector3 spineBase;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 ankleLeftPrev;
    private Vector3 ankleRightPrev;
    private Vector3 ankleMoving;

    private Vector3 legLeft;
    private Vector3 legRight;

    public float ankleLeftTravel;
    public float ankleRightTravel;


    public float rate;    
    public float direction;
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

                if (rate > minimumRate)
                {
                    // feet apart
                    if (!feetApart)
                    {
                        // feet first frame apart
                        feetApart = true;

                        direction = Mathf.Abs(ankleLeftTravel) > Mathf.Abs(ankleRightTravel) ?
                            -Mathf.Sign(ankleLeftTravel) : -Mathf.Sign(ankleRightTravel);
                    }
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

    private void OnValidate()
    {
        if (minimumRate > maximumRate) maximumRate = minimumRate;
    }
}
