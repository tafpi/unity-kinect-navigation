﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPlayer : MonoBehaviour
{
    //public GameObject[] pickupGroups;
    public GameObject logger;
    public LogSystem logSystem;
    public int interactionTimeNeeded = 2;
    [Range(0f, 1f)] public float viewportFactor = 0.35f;
    public GameObject player;
    public Camera cam;
    public bool drawTarget;
    public Color targetColor;
    [HideInInspector] public int picked;
    [HideInInspector] public float totalTimeSearching;

    private void Awake()
    {
        logSystem = logger.GetComponent<LogSystem>();
        cam = GetComponentInParent<Camera>();
        player = cam.transform.parent.gameObject;
        foreach (var pickupGroup in logSystem.pickupGroups)
        {
            foreach (Transform pickup in pickupGroup.transform)
            {
                PickupObject pickupObject = pickup.GetComponent<PickupObject>();
                pickupObject.pickupPlayer = this;
            }
        }
    }

    void Start()
    {

    }

    private void Update()
    {
        //logSystem.runLogger.pickups = picked;
        //logSystem.runLogger.pickupSearch = totalTimeSearching;
    }

    public bool PickupInTarget(Vector3 pickupPos, Renderer renderer)
    {
        // check if pickup object's position relative to camera viewport is in bounds of camera's target
        if (renderer.isVisible)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(pickupPos);
            return (viewPos.x > (1 - viewportFactor) / 2 && viewPos.x < (1 + viewportFactor) / 2 && viewPos.y > (1 - viewportFactor) / 2 && viewPos.y < (1 + viewportFactor) / 2);
        }
        else
        {
            return false;
        }
    }

    public void TotalSearchTime()
    {
        float total = 0;
        foreach (var pickupGroup in logSystem.pickupGroups)
        {
            foreach (Transform pickup in pickupGroup.transform)
            {
                PickupObject pickupObject = pickup.GetComponent<PickupObject>();
                //logSystem.runLogger.pickupSearch  += pickupObject.searchDuration;
                total += pickupObject.searchDuration;
            }
        }
        logSystem.runLogger.pickupSearch = total;
    }

    private void OnDrawGizmos()
    {
        Camera c = GetComponentInParent<Camera>();
        if (drawTarget)
        {
            // draw a target: part of the viewport with same aspect ratio and dimensions defined by factoring. ie: factor 1 means same dimensions as viewport
            Gizmos.color = targetColor;
            Gizmos.matrix = Matrix4x4.TRS(c.transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, c.fieldOfView * viewportFactor, 10f, 10.5f, c.aspect);
        }
        
    }
}