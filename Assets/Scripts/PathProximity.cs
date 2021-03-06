﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathProximity : MonoBehaviour
{
    // input
    public LogSystem logSystem;
    private float pathDist;
    [Range(0, 30)] public int pathDistMin;
    [Range(0, 30)] public int pathDistMax;
    private float t;
    [Range(0, 200)] public int pathCheckpoints;

    // calculations
    private int posOnPath;
    private int pathTraveled;
    private float proximityRate;
    private int proximityRange;
    private float currentProximity;

    // temp vars
    private Vector3 nearestPoint;
    private float proximityRates;

    // output
    public float[] proximityCheckpoints;
    public int proximityPercentage;

    private void Start()
    {
        proximityCheckpoints = new float[pathCheckpoints];
        proximityRange = pathDistMax - pathDistMin;
    }

    void Update()
    {
        // proximity in current position
        nearestPoint = logSystem.path.FindNearestPointTo(logSystem.player.transform.position, out t);
        nearestPoint.y = logSystem.player.transform.position.y;
        pathDist = Mathf.Clamp(Vector3.Distance(nearestPoint, logSystem.player.transform.position), pathDistMin, pathDistMax);
        proximityRate = (pathDistMax-pathDist)/proximityRange;
            
        // fill checkpoint proximity
        posOnPath = Mathf.FloorToInt(t * pathCheckpoints );
        currentProximity = proximityCheckpoints[posOnPath];
        if (currentProximity == 0 || (currentProximity != 0 && currentProximity > proximityRate))
        {
            proximityCheckpoints[posOnPath] = proximityRate;
        }
        pathTraveled = posOnPath > pathTraveled ? posOnPath : pathTraveled;

        // calculate proximity percentage
        proximityPercentage = 100;
        if (pathTraveled>0)
        {
            proximityRates = 0f;
            for (int i = 0; i < pathTraveled; i++)
            {
                proximityRates += proximityCheckpoints[i];
            }
            proximityPercentage = Mathf.FloorToInt(proximityRates*100/pathTraveled);
        }

    }

    private void OnValidate()
    {
        if (pathDistMin > pathDistMax) pathDistMax = pathDistMin;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i <= pathCheckpoints; i++)
        {
            Gizmos.color = Color.magenta;
            if (logSystem.path)
                Gizmos.DrawWireSphere(logSystem.path.GetPoint((float)i / pathCheckpoints), .5f);
        }            
    }
}
