using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GesturePointForward : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumRate = 40;
    public float maximumRate = 65;
    public float minimumRateBack = 30;
    public float maximumRateBack = 50;

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

    public float rate;

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

                    // (wrist-shoulder distance) / (arm full length). spine shoulder instead of shoulder for simpler calcs.
                    armTension = (spineShoulder - wrist).magnitude / ( (spineShoulder - elbow).magnitude + (elbow - wrist).magnitude );
                    
                    arm = wrist - spineShoulder;
                    arm.x = 0;

                    angle = Vector3.Angle(arm, Vector3.down);

                    rate = angle * armTension;

                    if ( wrist.z < spineShoulder.z)
                    {
                        rate = Functions.limitValue(minimumRate, maximumRate, rate);
                        gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope);
                    }
                    else
                    {
                        rate = Functions.limitValue(minimumRateBack, maximumRateBack, rate);
                        gestureRate = -Mathf.Pow((rate - minimumRateBack) / (maximumRateBack - minimumRateBack), slope);
                    }

                    if (state != null) state.gestureRate = gestureRate;

                    break;
                }
            }

        }


    }

}
