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
    private Vector3 shoulderLeft;
    private Vector3 shoulderRight;
    private Vector3 elbowLeft;
    private Vector3 elbowRight;

    public Vector3 forearm;
    public float wristShoulderDistance;
    public float wristLevel;
    //public Vector3 spine;

    //public float minimumAngle = 50f;
    //public float maximumAngle = 100f;
    //public float angle;
    public float rate;
    public float minimumRate = 0.04f;
    public float maximumRate = 0.1f;
    
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
                shoulderLeft = Functions.unityVector3(body.Joints[JointType.ShoulderLeft].Position);
                shoulderRight = Functions.unityVector3(body.Joints[JointType.ShoulderRight].Position);
                //elbowLeft = Functions.unityVector3(body.Joints[JointType.ElbowLeft].Position);
                //elbowRight = Functions.unityVector3(body.Joints[JointType.ElbowRight].Position);

                //forearm = wristRight - elbowRight;
                wristShoulderDistance = (shoulderRight - wristRight).sqrMagnitude;
                wristLevel = wristRight.y;
                if ( wristLeft.z < wristRight.z )
                {
                    // left hand forward
                    //forearm = wristLeft - elbowLeft;
                    wristShoulderDistance = (shoulderLeft - wristLeft).sqrMagnitude;
                    wristLevel = wristLeft.y;
                }
                //angle = 180 - Vector3.Angle(forearm, Functions.spine(body));
                //angle = Functions.limitValue(minimumAngle, maximumAngle, angle);
                //gestureRate = Mathf.Pow((angle - minimumAngle) / (maximumAngle - minimumAngle), slope);
                rate = wristShoulderDistance * wristLevel;
                rate = Functions.limitValue(minimumRate, maximumRate, rate);
                gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope);

                break;
            }
        }

    }

}
