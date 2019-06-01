using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GesturePointSideways1 : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public float gestureRate;

    public float slope = 1.2f;
    public float minimumRate = 40;
    public float maximumRate = 65;

    private Vector3 wristLeft;
    private Vector3 wristRight;
    private Vector3 elbowLeft;
    private Vector3 elbowRight;
    private Vector3 shoulderLeft;
    private Vector3 shoulderRight;
    
    private Vector3 spineShoulder;
    private Vector3 spineBase;

    private float armTension;
    private float angle;
    private int direction;
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
            Debug.Log("point sideways tracked");
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
                    spineShoulder = Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position);
                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);

                    wristLeft = Functions.unityVector3(body.Joints[JointType.WristLeft].Position);
                    wristRight = Functions.unityVector3(body.Joints[JointType.WristRight].Position);
                    elbowLeft = Functions.unityVector3(body.Joints[JointType.ShoulderLeft].Position);
                    elbowRight = Functions.unityVector3(body.Joints[JointType.ShoulderRight].Position);
                    shoulderLeft = Functions.unityVector3(body.Joints[JointType.ShoulderLeft].Position);
                    shoulderRight = Functions.unityVector3(body.Joints[JointType.ShoulderRight].Position);

                    if(rateFunc(wristLeft, elbowLeft, shoulderLeft) > rateFunc(wristRight, elbowRight, shoulderRight)){
                        //left
                        rate = rateFunc(wristLeft, elbowLeft, shoulderLeft);
                        direction = -1;
                    }
                    else
                    {
                        //right
                        rate = rateFunc(wristRight, elbowRight, shoulderRight);
                        direction = 1;
                    }
                    rate = Functions.limitValue(minimumRate, maximumRate, rate);
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;

                    if (state != null) state.gestureRate = gestureRate;

                    break;

                    float rateFunc(Vector3 wrist, Vector3 elbow, Vector3 shoulder)
                    {
                        armTension = (shoulder - wrist).magnitude / ((shoulder - elbow).magnitude + (elbow - wrist).magnitude);
                        angle = Vector3.Angle(wrist - shoulder, spineBase - shoulder);
                        return angle * armTension;
                    }
                    
                }
            }
        }
    }

    //private float rateNorm(float rate, float min, float max, float slope)
    //{
    //    rate = Functions.limitValue(minimumRate, maximumRate, rate);
    //    return Mathf.Pow((rate - min) / (max - min), slope);
    //    //if ( (rate >= min) && (rate <= max) ){
    //    //    //return Mathf.Pow((rate - min / max) / (1 - (min / max)), slope);
    //    //    //return Mathf.Pow((rate*max - min)/(max-min), slope);
    //    //} else {
    //    //    return 0;
    //    //}
    //}

}