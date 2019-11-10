﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool canMove = true;
    public bool travelling;    
    public bool rotatingY;
    public bool rotatingX;
    public bool rotatingWhileTravelling;

    private Stopwatch travelStopwatch;
    private Stopwatch rotateYStopwatch;
    private Stopwatch rotateXStopwatch;
    private Stopwatch rotateWhileTravellingStopwtach;

    public LogRun logRun;

    private void Awake()
    {
        travelStopwatch = new Stopwatch();
        rotateYStopwatch = new Stopwatch();
        rotateXStopwatch = new Stopwatch();
        rotateWhileTravellingStopwtach = new Stopwatch();
    }

    void Start()
    {

    }
    
    void Update()
    {
        if (travelling)
        {
            if (!travelStopwatch.IsRunning) travelStopwatch.Start();

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
            if (travelStopwatch.IsRunning)
            {
                travelStopwatch.Stop();
                logRun.travelDuration = travelStopwatch.ElapsedMilliseconds;
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
    }
    
}
