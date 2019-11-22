using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool canMove = true;
    public bool travelling;
    public bool travellingBackwards;
    public bool rotatingY;
    public bool rotatingX;
    public bool rotatingWhileTravelling;
    public bool idle;

    private Stopwatch totalTravelStopwatch;
    private Stopwatch backwardsTravelStopwatch;
    private Stopwatch rotateYStopwatch;
    private Stopwatch rotateXStopwatch;
    private Stopwatch rotateWhileTravellingStopwtach;
    private Stopwatch idleStopwatch;

    public LogRun logRun;

    private void Awake()
    {
        totalTravelStopwatch = new Stopwatch();
        backwardsTravelStopwatch = new Stopwatch();
        rotateYStopwatch = new Stopwatch();
        rotateXStopwatch = new Stopwatch();
        rotateWhileTravellingStopwtach = new Stopwatch();
        idleStopwatch = new Stopwatch();
    }

    void Start()
    {

    }
    
    void Update()
    {
        if (logRun)
        {
            if (travelling)
            {
                if (!totalTravelStopwatch.IsRunning)
                    totalTravelStopwatch.Start();
                if (!backwardsTravelStopwatch.IsRunning && travellingBackwards)
                    backwardsTravelStopwatch.Start();

                if (rotatingX || rotatingY)
                {
                    rotatingWhileTravelling = true;
                    if (!rotateWhileTravellingStopwtach.IsRunning) rotateWhileTravellingStopwtach.Start();
                }
                else
                {
                    rotatingWhileTravelling = false;
                    if (rotateWhileTravellingStopwtach.IsRunning)
                    {
                        rotateWhileTravellingStopwtach.Stop();
                        logRun.rotateWhileTravelDuration = rotateWhileTravellingStopwtach.ElapsedMilliseconds;
                    }
                }
            }
            else
            {
                if (totalTravelStopwatch.IsRunning)
                {
                    totalTravelStopwatch.Stop();
                    logRun.totalTravelDuration = totalTravelStopwatch.ElapsedMilliseconds;
                    logRun.timesStopped++;
                }
                if (backwardsTravelStopwatch.IsRunning)
                {
                    backwardsTravelStopwatch.Stop();
                    logRun.backwardsTravelDuration = backwardsTravelStopwatch.ElapsedMilliseconds;
                }
            }

            if (rotatingY)
            {
                if (!rotateYStopwatch.IsRunning) rotateYStopwatch.Start();
            }
            else
            {
                if (rotateYStopwatch.IsRunning)
                {
                    rotateYStopwatch.Stop();
                    logRun.rotateYDuration = rotateYStopwatch.ElapsedMilliseconds;
                }
            }

            if (rotatingX)
            {
                if (!rotateXStopwatch.IsRunning) rotateXStopwatch.Start();
            }
            else
            {
                if (rotateXStopwatch.IsRunning)
                {
                    rotateXStopwatch.Stop();
                    logRun.rotateXDuration = rotateXStopwatch.ElapsedMilliseconds;
                }
            }

            if (!travelling && !rotatingX && !rotatingY)
            {
                idle = true;
                if (!idleStopwatch.IsRunning) idleStopwatch.Start();
            }
            else
            {
                if (idleStopwatch.IsRunning)
                {
                    idleStopwatch.Stop();
                    logRun.idleControllerDuration = idleStopwatch.ElapsedMilliseconds;
                }
            }
        }
    }
    
}
