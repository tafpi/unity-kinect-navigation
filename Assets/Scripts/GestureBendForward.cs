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
    public float currentAngle;

    private Vector3 hipLeft;
    private Vector3 hipRight;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 ankle;
    private Vector3 spineBase;
    private Vector3 spineShoulder;

    private Vector3 spine;
    private Vector3 leg;
    public float angle;
    public float minimumAngle = 154f;
    public float maximumAngle = 172f;
    public float minimumAngleBack = 165f;
    public float maximumAngleBack = 170f;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("BendForward gesture can be recognised");
    }

    // Update is called once per frame
    void Update()
    {

        if (trackGesture)
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
                    hipLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    hipRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position);
                    ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);
                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);
                    spineShoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);

                    spine = spineShoulder - spineBase;
                    // leg with most tension defines the angle
                    if ((ankleLeft - hipLeft).sqrMagnitude < (ankleRight - hipRight).sqrMagnitude)
                    {
                        leg = ankleRight - hipRight;
                        ankle = ankleRight;
                    }
                    else
                    {
                        leg = ankleLeft - hipLeft;
                        ankle = ankleLeft;
                    }

                    angle = Vector3.Angle(spine, leg);

                    if (spineShoulder.z < spineBase.z)
                    {
                        angle = Functions.limitValue(minimumAngle, maximumAngle, angle);
                        gestureRate = Mathf.Pow((maximumAngle - angle) / (maximumAngle - minimumAngle), slope);
                    }
                    else
                    {
                        angle = Functions.limitValue(minimumAngleBack, maximumAngleBack, angle);
                        gestureRate = -Mathf.Pow((maximumAngleBack - angle) / (maximumAngleBack - minimumAngleBack), slope);
                    }
                    break;
                }
            }
        }

    }

}
