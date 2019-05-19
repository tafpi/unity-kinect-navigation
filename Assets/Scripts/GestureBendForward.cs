using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureBendForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumAngle = 140f;
    public float maximumAngle = 165f;
    public float currentAngle;

    private Vector3 spine;
    private Vector3 leg;
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
                if(body.Joints[JointType.SpineShoulder].Position.Z < body.Joints[JointType.SpineBase].Position.Z)
                {
                    spine = Functions.spine(body);
                    leg = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position) - Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    if (    ( Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position) - Functions.unityVector3(body.Joints[JointType.HipLeft].Position) ).sqrMagnitude <
                            ( Functions.unityVector3(body.Joints[JointType.AnkleRight].Position) - Functions.unityVector3(body.Joints[JointType.HipRight].Position) ).sqrMagnitude    )
                    {
                        leg = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position) - Functions.unityVector3(body.Joints[JointType.HipRight].Position);
                    }

                    currentAngle = Vector3.Angle(spine, leg);

                    angle = Functions.limitValue(minimumAngle, maximumAngle, currentAngle);

                    gestureRate = Mathf.Pow((maximumAngle - angle) / (maximumAngle - minimumAngle), slope);
                }
                break;
            }
        }

    }

}
