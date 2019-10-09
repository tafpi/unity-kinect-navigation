﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWallTrigger : MonoBehaviour
{

    public float collisionDuration;
    public float collisionEnterTime;
    public GameObject player;
    private LogCollisions collisionLogger;
    
    void Start()
    {
        ResetWall();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
            Debug.Log("entered trigger");
            collisionLogger.CollisionBegin(this);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
            collisionLogger.CollisionUpdate(this);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
            collisionLogger.CollisionEnd(this);
        }
    }

    public void AssignPlayer(GameObject playerObject)
    {
        player = playerObject;
        collisionLogger = player.GetComponent<LogCollisions>();
    }

    public void ResetWall()
    {
        collisionDuration = 0;
        collisionEnterTime = 0;
    }

}
