using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureHalfStepForward : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public bool trackGesture;
    
    public float gestureRate;

    public float slope = 1.2f;
    public string footForward;
    public Vector3 playerCenter = new Vector3(0,0,0);
    public float freeMovementRange = 0.02f;
    public float playerCenterDiff;

    public Vector3 leftAnkle;
    public Vector3 rightAnkle;
    public float groundThreshold = 0.1f;
    public bool feetDown;
    private float legLength;
    private float feetDistance;
    public float ratio;
    public float minimumRatio = 0.15f;
    public float maximumRatio = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                //if( playerCenter == new Vector3(0,0,0))
                //{
                //    playerCenter = Functions.unityVector3(body.Joints[JointType.SpineBase].Position);
                //}
                //else
                //{
                //    playerCenterDiff = (playerCenter - Functions.unityVector3(body.Joints[JointType.SpineBase].Position)).sqrMagnitude;
                //    if ( playerCenterDiff > freeMovementRange )
                //    {
                //        Debug.Log("out of free movement range");
                //    }
                //}
                //if (
                //        body.Joints[JointType.AnkleLeft].Position.Z < body.Joints[JointType.AnkleRight].Position.Z
                //    )
                //{
                //    footForward = "left";
                //}
                //else
                //{
                //    footForward = "right";
                //}
                
                leftAnkle = Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position);
                rightAnkle = Functions.unityVector3(body.Joints[JointType.AnkleRight].Position);
                if (Mathf.Abs(leftAnkle.y - rightAnkle.y) < groundThreshold)
                {
                    // both feet on the ground: ankles height (y) distance is within threshold
                    feetDown = true;
                    if( Mathf.Abs(leftAnkle.z - rightAnkle.z) > Mathf.Abs(leftAnkle.x - rightAnkle.x))
                    {
                        // depth (z) feet distance greater than horizontal (x) feet distance
                        legLength = (Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position) - Functions.unityVector3(body.Joints[JointType.KneeLeft].Position)).sqrMagnitude
                                +   (Functions.unityVector3(body.Joints[JointType.HipLeft].Position) - Functions.unityVector3(body.Joints[JointType.KneeLeft].Position)).sqrMagnitude;
                        feetDistance = (Functions.unityVector3(body.Joints[JointType.AnkleLeft].Position) - Functions.unityVector3(body.Joints[JointType.AnkleRight].Position)).sqrMagnitude;
                        ratio = feetDistance / legLength;
                        ratio = Functions.limitValue(minimumRatio, maximumRatio, ratio);

                        gestureRate = Mathf.Pow((ratio - minimumRatio) / (maximumRatio - minimumRatio), slope);
                    }
                }
                else
                {
                    // ankles height (y) distance is out of threshold
                    feetDown = false;
                    gestureRate = 0;
                }
                break;
            }
        }

    }

    private Vector3 getPlayerCenter(Body body)
    {
        return new Vector3(
            body.Joints[JointType.SpineBase].Position.X,
            Mathf.Abs(body.Joints[JointType.AnkleLeft].Position.Y - body.Joints[JointType.AnkleRight].Position.Y) / 2,
            Mathf.Abs(body.Joints[JointType.AnkleLeft].Position.Z - body.Joints[JointType.AnkleRight].Position.Z) / 2
        );
    }

}
