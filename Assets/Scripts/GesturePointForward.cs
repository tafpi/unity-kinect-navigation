using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GesturePointForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;
    public float slope = 1.2f;

    private Vector3 wristLeft;
    private Vector3 wristRight;
    private Vector3 wrist;
    private Vector3 shoulder;

    public Vector3 forearm;
    public float wristShoulderDistance;
    public float wristLevel;

    public float rate;
    public float minimumRate = 0.06f;
    public float maximumRate = 0.16f;
    public float minimumRateBack = 0.05f;
    public float maximumRateBack = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
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
                wristLeft = Functions.unityVector3(body.Joints[JointType.WristLeft].Position);
                wristRight = Functions.unityVector3(body.Joints[JointType.WristRight].Position);
                shoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);
                // which wrist is further from the body
                wrist = wristLeft;
                if ( Mathf.Abs(wristLeft.z - shoulder.z) < Mathf.Abs(wristRight.z - shoulder.z) )
                {
                    wrist = wristRight;
                }

                wristShoulderDistance = (shoulder - wrist).sqrMagnitude;
                wristLevel = Mathf.Abs(wrist.y);

                rate = wristShoulderDistance * wristLevel;
                if ( wrist.z < shoulder.z)
                {
                    rate = Functions.limitValue(minimumRate, maximumRate, rate);
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope);
                }
                else
                {
                    rate = Functions.limitValue(minimumRateBack, maximumRateBack, rate);
                    gestureRate = - Mathf.Pow((rate - minimumRateBack) / (maximumRateBack - minimumRateBack), slope);
                }

                break;
            }
        }

    }

}
