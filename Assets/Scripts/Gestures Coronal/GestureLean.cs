﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureLean : MonoBehaviour
{
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    private bool trackGesture = false;

    public float gestureRate;

    [Range(1f, 2f)] public float slope = 1.2f;
    [Range(0f, 1f)] public float minimumRate = 0.2f;
    [Range(0f, 1f)] public float maximumRate = 0.5f;

    private Vector3 spineBase;
    private Vector3 spineShoulder;
    private Vector3 head;

    private Vector3 spine;
    private Vector3 spineVertical;
    private Vector3 neck;
    private Vector3 neckVertical;
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
                    head = Functions.unityVector3(body.Joints[JointType.Head].Position);

                    spine = spineShoulder - spineBase;
                    spine = new Vector3(spine.x, spine.y, 0);
                    spineVertical = new Vector3(0, spine.magnitude, 0);

                    neck = head - spineShoulder;
                    neck = new Vector3(neck.x, neck.y, 0);
                    neckVertical = new Vector3(0, neck.magnitude, 0);

                    rate = Mathf.Sin(Vector3.Angle(spine, spineVertical) * Mathf.Deg2Rad) +
                        Mathf.Sin(Vector3.Angle(neck, neckVertical) * Mathf.Deg2Rad);

                    direction = Mathf.Sign((spine - spineVertical).x);

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
    }
}
