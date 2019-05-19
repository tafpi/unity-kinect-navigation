using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureBendKneeForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;

    public float minimumAngle = 140f;
    public float maximumAngle = 165f;
    public float slope = 1.2f;
    private Vector3 shinLeft;
    private Vector3 shinRight;
    private Vector3 thighLeft;
    private Vector3 thighRight;
    private float angleLeft;
    private float angleRight;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("BendForward gesture can be recognised");
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
                shinLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position) - Functions.unityVector3(body.Joints[JointType.KneeLeft].Position);
                shinRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position) - Functions.unityVector3(body.Joints[JointType.KneeRight].Position);
                thighLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position) - Functions.unityVector3(body.Joints[JointType.KneeLeft].Position);
                thighRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position) - Functions.unityVector3(body.Joints[JointType.KneeRight].Position);
                angleLeft = Vector3.Angle(shinLeft, thighLeft);
                angleRight = Vector3.Angle(shinRight, thighRight);

                if(angleLeft <= angleRight)
                {
                    angle = angleLeft;
                }
                else
                {
                    angle = angleRight;
                }

                if (angle > maximumAngle)
                {
                    angle = maximumAngle;
                }
                if (angle < minimumAngle)
                {
                    angle = minimumAngle;
                }

                gestureRate = Mathf.Pow(  ( maximumAngle - angle ) / ( maximumAngle - minimumAngle ), slope);
                
                break;
            }
        }

    }

}
