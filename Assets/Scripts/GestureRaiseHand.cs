using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class GestureRaiseHand : MonoBehaviour
{
    //public Windows.Kinect.JointType _rightShoulderJoint;
    public GameObject _bodySourceManager;
    private BodySourceManager _bodyManager;
    public float gestureRate;
    public bool trackGesture;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("RaiseHand gesture can be recognised");
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
                //rightHandY = body.Joints[JointType.HandRight].Position.Y;
                //this.gameObject.transform.position = new Vector3
                // this.gameObject.transform.localPosition =  body.Joints[_jointType].Position;
                //var pos = body.Joints[_jointType].Position;
                //this.gameObject.transform.position = new Vector3(pos.X, pos.Y, pos.Z);

                gestureRate = body.Joints[JointType.HandRight].Position.Y;

                break;
            }
        }

    }
}
