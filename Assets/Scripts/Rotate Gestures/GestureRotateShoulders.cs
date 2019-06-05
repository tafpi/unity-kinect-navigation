using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureRotateShoulders : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public float gestureRate;

    public float slope = 1.2f;
    public float minimumRate = 0.45f;
    public float maximumRate = 0.7f;

    private Vector3 shoulderLeft;
    private Vector3 shoulderRight;
    private Vector3 hipLeft;
    private Vector3 hipRight;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;

    private Vector3 horizontalShoulders;
    private Vector3 horizontalHips;
    //private Vector3 horizontalAnkles;
    private float direction;
    public float rate;

    //private float angleTop;
    //private float angleBot;
    //private float directionTop;
    //private float directionBot;
    //private float rateTop;
    //private float rateBot;

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
                    shoulderLeft = Functions.unityVector3(body.Joints[JointType.ShoulderLeft].Position);
                    shoulderRight = Functions.unityVector3(body.Joints[JointType.ShoulderRight].Position);
                    hipLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    hipRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position);
                    //ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    //ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);

                    horizontalShoulders = shoulderLeft - shoulderRight;
                    horizontalShoulders.y = 0;
                    horizontalHips = hipLeft - hipRight;
                    horizontalHips.y = 0;
                    //horizontalAnkles = ankleLeft - ankleRight;
                    //horizontalAnkles.y = 0;

                    //directionTop = Mathf.Sign(((hipLeft - hipRight) - (shoulderLeft - shoulderRight)).z);
                    //rateTop = Mathf.Sin(Vector3.Angle(horizontalHips, horizontalShoulders) * Mathf.Deg2Rad) * directionTop;
                    //directionBot = Mathf.Sign(((ankleLeft - ankleRight) - (hipLeft - hipRight)).z);
                    //rateBot = Mathf.Sin(Vector3.Angle(horizontalAnkles, horizontalHips) * Mathf.Deg2Rad) * directionTop;

                    direction = Mathf.Sign(((hipLeft - hipRight) - (shoulderLeft - shoulderRight)).z);
                    rate = Mathf.Sin(Vector3.Angle(horizontalHips, horizontalShoulders) * Mathf.Deg2Rad);

                    rate = Functions.limitValue(minimumRate, maximumRate, rate) * direction;
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope);

                    if (state != null) state.gestureRate = gestureRate;

                    break;

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