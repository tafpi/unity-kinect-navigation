using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GesturePointUpDown : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public GameObject player;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumAngle = 30;
    public float maximumAngle = 65;

    private Vector3 wristLeft;
    private Vector3 wristRight;
    private Vector3 wrist;
    private Vector3 elbow;
    private Vector3 spineShoulder;
    private Vector3 spineBase;
    private Vector3 arm;

    public float wristShoulderDistance;
    public float armLength;
    public float armTension;
    public float angle;
    public bool controlling; // is controlling the view

    public float rate;

    private GestureState state;

    // Start is called before the first frame update
    void Start()
    {
        state = gameObject.GetComponent<GestureState>();
        trackGesture = gameObject.GetComponent<GestureState>().gestureTracked;
        controlling = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (trackGesture)
        {
            Debug.Log("point forward tracked");
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
                    spineShoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);
                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);

                    // which wrist is further from the body
                    if ( Mathf.Abs(wristLeft.z - spineShoulder.z) < Mathf.Abs(wristRight.z - spineShoulder.z) )
                    {
                        wrist = wristRight;
                        elbow = Functions.unityVector3(body.Joints[JointType.ElbowRight].Position);
                    }
                    else
                    {
                        wrist = wristLeft;
                        elbow = Functions.unityVector3(body.Joints[JointType.ElbowLeft].Position);
                    }
                    
                    armTension = (spineShoulder - wrist).magnitude / ( (spineShoulder - elbow).magnitude + (elbow - wrist).magnitude );
                    arm = wrist - spineShoulder;
                    arm.x = 0;
                    angle = Vector3.Angle(arm, Vector3.down) - 90;

                    if(angle < -maximumAngle)
                    {
                        rate = 0;
                    } else
                    {
                        rate = angle * armTension;
                        rate = Mathf.Sign(angle) * Functions.limitValue(minimumAngle, maximumAngle, Mathf.Abs(rate));
                    }

                    gestureRate = -Mathf.Sign(angle) * Mathf.Pow((Mathf.Abs(rate) - minimumAngle) / (maximumAngle - minimumAngle), slope);

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }
            }

        }


    }

}
