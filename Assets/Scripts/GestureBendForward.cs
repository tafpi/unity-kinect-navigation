using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureBendForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float currentAngle;

    private Vector3 hipLeft;
    private Vector3 hipRight;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 ankle;
    private Vector3 spineBase;
    private Vector3 spineShoulder;

    private Vector3 spine;
    //private Vector3 leg;
    //public float angle;
    public float distance;
    public float rate;
    //public float minimumAngle = 154f;
    //public float maximumAngle = 172f;
    //public float minimumAngleBack = 165f;
    //public float maximumAngleBack = 170f;
    public float minimumRate = 0.22f;
    public float maximumRate = 0.4f;
    public float minimumRateBack = 0.09f;
    public float maximumRateBack = 0.14f;

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
            Debug.Log("bend forward tracked");
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

                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);
                    spineShoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);
                    distance = spineBase.z - spineShoulder.z;
                    spine = spineShoulder - spineBase;
                    rate = distance / spine.magnitude;
                    gestureRate = 0;
                    if(rate > minimumRate)
                    {
                        //bending forwards
                        Debug.Log("bending forwards");
                        rate = Functions.limitValue(minimumRate, maximumRate, rate);
                        gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope);
                    }
                    if (rate < -minimumRateBack)
                    {
                        //bending backwards
                        rate = Functions.limitValue(minimumRateBack, maximumRateBack, Mathf.Abs(rate));
                        gestureRate = -Mathf.Pow((rate - minimumRateBack) / (maximumRateBack - minimumRateBack), slope);
                    }
                    if (state != null) state.gestureRate = gestureRate;
                    break;

                    //hipLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    //hipRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position);
                    //ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    //ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);
                    //spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);
                    //spineShoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);
                    //spine = spineShoulder - spineBase;
                    //// leg with most tension defines the angle
                    //if ((ankleLeft - hipLeft).sqrMagnitude < (ankleRight - hipRight).sqrMagnitude)
                    //{
                    //    leg = ankleRight - hipRight;
                    //    ankle = ankleRight;
                    //}
                    //else
                    //{
                    //    leg = ankleLeft - hipLeft;
                    //    ankle = ankleLeft;
                    //}

                    //angle = Vector3.Angle(spine, leg);
                    //gestureRate = 0;
                    ////if ( (spineShoulder.z < spineBase.z) && (angle < minimumAngle) )
                    //if (spineShoulder.z < spineBase.z)
                    //{
                    //    Debug.Log(1);
                    //    angle = Functions.limitValue(maximumAngle, minimumAngle, angle);
                    //    gestureRate = 1 - Mathf.Pow((angle - maximumAngle) / (minimumAngle - maximumAngle), slope);
                    //}
                    ////if ( (spineShoulder.z > spineBase.z) && () )
                    //else
                    //{
                    //    Debug.Log(-1);
                    //    angle = Functions.limitValue(minimumAngleBack, maximumAngleBack, angle);
                    //    gestureRate = -Mathf.Pow((maximumAngleBack - angle) / (maximumAngleBack - minimumAngleBack), slope);
                    //}

                    //if (state != null) state.gestureRate = gestureRate;

                    //break;

                }
            }
        }

    }

}
