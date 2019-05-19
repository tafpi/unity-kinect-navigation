using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class Functions
{
    private static float multiplier;

    public static float Multiply(float a, float b)
    {
        return a * b;
    }

    public static Vector3 unityVector3(CameraSpacePoint point)
    {
        return new Vector3(point.X, point.Y, point.Z); //swap the parameters around as you see fit
    }

    public static float gestureMultiplier(float min, float max, float rate, float slope)
    {
        if ( (rate >= 0) && (rate <= 1) )
        {
            if ( (rate >= min) && (rate <= max) )
            {
                multiplier = Mathf.Pow((rate - min) / (max - min), slope);
            }
            else
            {
                multiplier = 0;
            }
        } else
        {
            Debug.Log("gesture rate out of scope 0-1");
        }
        return multiplier;
    }

    public static float limitValue(float min, float max, float value)
    {
        //value = value > max ? max : min;
        if (value > max)
        {
            value = max;
        }
        if (value < min)
        {
            value = min;
        }
        return value;
    }

    public static bool feetGrounded(Body body, float threshold)
    {
        // Assuming at least one foot is grounded. Won't work if jumping and both ankles at same height
        return Mathf.Abs(body.Joints[JointType.AnkleLeft].Position.Y - body.Joints[JointType.AnkleRight].Position.Y) < threshold;
    }

    public static Vector3 spine(Body body)
    {
        // Assuming insignificant mid-spine angle (spine is 3 joints: base, mid and shoulder)
        return Functions.unityVector3(body.Joints[JointType.SpineShoulder].Position) - Functions.unityVector3(body.Joints[JointType.SpineBase].Position);
    }

}
