using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureHalfStepForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumRatio = 0.3f;
    public float maximumRatio = 0.55f;

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
    private Vector3 feetMiddle;
    public float playerCenterDiff;
    public float direction;
    public bool feetApart;
    public float groundThreshold = 0.1f;
    public float distanceThreshold = 0.1f;
    public bool feetDown;
    private float legLength;
    private float feetDistance;
    public float ratio;

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
                    //if( playerCenter == new Vector3(0,0,0))
                    //{
                    //    playerCenter = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);
                    //}
                    //else
                    //{
                    //    playerCenterDiff = (playerCenter - Functions.unityVector3(body.Joints[JointType.SpineBase].Position)).sqrMagnitude;
                    //    if ( playerCenterDiff > freeMovementRange )
                    //    {
                    //        Debug.Log("out of free movement range");
                    //    }
                    //}

                    gestureRate = 0;
                    ankleLeftPrev = ankleLeft;
                    ankleRightPrev = ankleRight;
                    ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);

                    if (Mathf.Abs(ankleLeft.z - ankleRight.z) > Mathf.Abs(ankleLeft.x - ankleRight.x))
                    {
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
                            if (!feetApart)
                            {
                                // first frame feet apart
                                ankleMoving = Mathf.Abs(ankleLeft.z - ankleLeftPrev.z) > Mathf.Abs(ankleRight.z - ankleRightPrev.z) ? ankleLeft : ankleRight;
                                direction = ankleMoving == ankleForward ? 1 : -1;
                            }
                            feetApart = true;
                            if (Mathf.Abs(ankleLeft.y - ankleRight.y) < groundThreshold)
                            {
                                // both feet on the ground: ankles height (y) distance is within threshold
                                ratio = Functions.limitValue(minimumRatio, maximumRatio, ratio);
                                gestureRate = Mathf.Pow((ratio - minimumRatio) / (maximumRatio - minimumRatio), slope) * direction;
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
                    }

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }
            }
        }

    }

}
