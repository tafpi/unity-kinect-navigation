using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class LimitWallCollision : MonoBehaviour
{
    public float TimeEnter { get; set; }
    public float TimeExit { get; set; }
    public float Duration { get; set; }
    public LimitWallCollision(float timeEnter)
    {
        TimeEnter = timeEnter;
        TimeExit = 0;
        Duration = 0;
    }
    private void IncrementDuration()
    {
        this.Duration = Time.deltaTime - this.TimeEnter;
    }
}