using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class Hand
{
    public CameraSpacePoint Wrist { get; set; }
    public CameraSpacePoint Elbow { get; set; }
    public Vector3 WristPoint { get; set; }
    public Vector3 ElbowPoint { get; set; }
    //public HandState Fist { get; set; }
    public Hand(CameraSpacePoint wrist, CameraSpacePoint elbow, Vector3 wristPoint, Vector3 elbowPoint)
    {
        Wrist = wrist;
        Elbow = elbow;
        WristPoint = wristPoint;
        ElbowPoint = elbowPoint;
        //Fist = fist;
    }
    public Hand SetHandPoints()
    {
        this.WristPoint = Functions.unityVector3(this.Wrist);
        this.ElbowPoint = Functions.unityVector3(this.Elbow);
        return this;
    }
}