using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureBendKneeOutwards : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public float gestureRate;

    public float slope = 3;
    public float minimumRate = 0.2f;
    public float maximumRate = 0.3f;

    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 kneeLeft;
    private Vector3 kneeRight;
    private Vector3 hipLeft;
    private Vector3 hipRight;

    private Vector3 thighLeft;
    private Vector3 thighRight;
    private Vector3 legLeft;
    private Vector3 legRight;

    private float rateLeft;
    private float rateRight;

    public float direction;
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

                    ankleLeft = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                    ankleRight = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);
                    kneeLeft = Functions.unityVector3(body.Joints[JointType.KneeLeft].Position);
                    kneeRight = Functions.unityVector3(body.Joints[JointType.KneeRight].Position);
                    hipLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    hipRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position);

                    thighLeft = kneeLeft - hipLeft;
                    thighLeft = new Vector3(thighLeft.x, thighLeft.y, 0);
                    legLeft = ankleLeft - hipLeft;
                    legLeft = new Vector3(legLeft.x, legLeft.y, 0);
                    thighRight = kneeRight - hipRight;
                    thighRight = new Vector3(thighRight.x, thighRight.y, 0);
                    legRight = ankleRight - hipRight;
                    legRight = new Vector3(legRight.x, legRight.y, 0);

                    rateLeft = Mathf.Sin(Vector3.Angle(thighLeft, legLeft) * Mathf.Deg2Rad);
                    rateRight = Mathf.Sin(Vector3.Angle(thighRight, legRight) * Mathf.Deg2Rad);

                    rate = rateLeft > rateRight ? rateLeft : rateRight;
                    if(rateLeft > rateRight)
                    {
                        rate = rateLeft;
                        direction = -1;
                    } else
                    {
                        rate = rateRight;
                        direction = 1;
                    }

                    rate = Functions.limitValue(minimumRate, maximumRate, rate);
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;

                    if (state != null) state.gestureRate = gestureRate;

                    break;

                }
            }
        }
    }
}
