using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureTwistUpperBody : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public float gestureRate;

    [Range(1f, 2f)] public float slope;
    [Range(0f, 1f)] public float minimumRate;
    [Range(0f, 1f)] public float maximumRate;

    private Vector3 shoulderLeft;
    private Vector3 shoulderRight;
    private Vector3 ankleLeft;
    private Vector3 ankleRight;

    private Vector3 horizontalShoulders;
    private Vector3 horizontalAnkles;
    private float direction;
    public float rate;
    
    private GestureState state;
    
    void Start()
    {
        state = gameObject.GetComponent<GestureState>();
        trackGesture = gameObject.GetComponent<GestureState>().gestureTracked;
    }
    
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
                    ankleLeft = Functions.unityVector3(body.Joints[JointType.HipLeft].Position);
                    ankleRight = Functions.unityVector3(body.Joints[JointType.HipRight].Position);

                    horizontalShoulders = shoulderLeft - shoulderRight;
                    horizontalShoulders = new Vector3(horizontalShoulders.x, 0, horizontalShoulders.z);
                    horizontalAnkles = ankleLeft - ankleRight;
                    horizontalAnkles = new Vector3(horizontalAnkles.x, 0, horizontalAnkles.z * 0.05f);

                    direction = Mathf.Sign((horizontalAnkles - (shoulderLeft - shoulderRight)).z);

                    rate = Mathf.Sin(Vector3.Angle(horizontalAnkles, horizontalShoulders) * Mathf.Deg2Rad);

                    rate = Functions.limitValue(minimumRate, maximumRate, rate);
                    gestureRate = Mathf.Pow((rate - minimumRate) / (maximumRate - minimumRate), slope) * direction;

                    if (state != null) state.gestureRate = gestureRate;

                    break;

                }
            }
        }
    }

    private void OnValidate()
    {
        if (minimumRate > maximumRate) maximumRate = minimumRate;
        //slope = Mathf.Floor(slope * 100) / 100;
    }

}