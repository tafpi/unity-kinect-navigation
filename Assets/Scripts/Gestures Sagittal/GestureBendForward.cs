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

    [Range(1f, 2f)] public float slope = 1.2f;
    [Range(0f, 1f)] public float minimumRate = 0.25f;
    [Range(0f, 1f)] public float maximumRate = 0.4f;
    [Range(0f, 1f)] public float minimumRateBack = 0.01f;
    [Range(0f, 1f)] public float maximumRateBack = 0.05f;

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
    private float rateNorm;

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
                    spineShoulder = Functions.unityVector3(body.Joints[JointType.Head].Position);
                    rate = Vector3.Angle(Vector3.forward, (spineShoulder - spineBase))/180;
                    gestureRate = 0;
                    if (rate > minimumRate)
                    {
                        //bending forwards
                        rateNorm = Functions.limitValue(minimumRate, maximumRate, rate);
                        gestureRate = Mathf.Pow((rateNorm - minimumRate) / (maximumRate - minimumRate), slope);
                    }
                    if (rate < maximumRateBack)
                    {
                        //bending backwards
                        rateNorm = Functions.limitValue(minimumRateBack, maximumRateBack, Mathf.Abs(rate));
                        gestureRate = -(1 - Mathf.Pow((rateNorm - minimumRateBack) / (maximumRateBack - minimumRateBack), slope));
                    }

                    if (state != null) state.gestureRate = gestureRate;
                    break;

                }
            }
        }

    }

    private void OnValidate()
    {
        if (minimumRate > maximumRate) maximumRate = minimumRate;
        if (minimumRateBack > maximumRateBack) maximumRateBack = minimumRateBack;
    }

}
