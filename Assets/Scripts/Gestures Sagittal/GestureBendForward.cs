using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureBendForward : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;
    
    public float gestureRate;

    public float slope = 1.2f;
    public float minimumRate = 0.22f;
    public float maximumRate = 0.4f;
    public float minimumRateBack = 0.09f;
    public float maximumRateBack = 0.14f;

    private Vector3 hipLeft;
    private Vector3 hipRight;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 ankle;
    private Vector3 spineBase;
    private Vector3 spineShoulder;

    private Vector3 spine;
    public float currentAngle;
    public float distance;
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

                }
            }
        }

    }

}
