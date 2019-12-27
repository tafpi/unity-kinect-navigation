using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureTranslateHips : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public float gestureRate;

    public float slope = 3;
    public float minimumRate = 0.1f;
    public float maximumRate = 0.17f;

    private Vector3 ankleLeft;
    private Vector3 ankleRight;
    private Vector3 spineBase;
    private Vector3 waist;
    private Vector3 waistReference;
    private Vector3 anklesMiddle;

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
                    spineBase = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);

                    anklesMiddle = (ankleRight + ankleLeft) / 2;
                    //if(ankleLeft.z > ankleRight.z)
                    //{
                    //    anklesMiddle = Vector3.Lerp(ankleLeft, ankleRight, 0.35f);
                    //} else
                    //{
                    //    anklesMiddle = Vector3.Lerp(ankleRight, ankleLeft, 0.35f);
                    //}
                    waist = spineBase - anklesMiddle;
                    waist = new Vector3(waist.x, waist.y, waist.z);
                    waistReference = new Vector3(0, waist.y, waist.z);

                    rate = Mathf.Sin(Vector3.Angle(waist, waistReference) * Mathf.Deg2Rad);

                    direction = Mathf.Sign((waist - waistReference).x);

                    rate = Functions.limitValue(minimumRate, maximumRate, rate);
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;

                    if (state != null) state.gestureRate = gestureRate;

                    break;

                }
            }
        }
    }
}
