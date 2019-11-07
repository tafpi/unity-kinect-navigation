using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogRun : MonoBehaviour
{
    // Attach as component to player

    // input
    public bool isIntroRun;

    // computed at start
    public int runId;
    public GameObject gestureTravel;
    public GameObject gestureRotateY;
    public GameObject gestureRotateX;
    public Time start;
    public int totalPickups;

    // computed on update
    public Time duration;
    public Time movingDuration;
    public Time idleDuration;
    public float distanceTraveled;
    public int proximityPercentage;
    public int timesStopped;
    public int obstaclesCollided;
    public int wallsCollided;
    public int fencesCollided;
    public int propsCollided;
    public int pickups;
    public Time pickupSearch;

    // computed on end
    public Time finish;
    public bool endReached;

}
