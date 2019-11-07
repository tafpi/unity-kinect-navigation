using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{

    [HideInInspector] public float collisionDuration;
    [HideInInspector] public float collisionEnterTime;
    private GameObject player;
    private LogCollisions collisionLogger;
    
    void Start()
    {
        ResetWall();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
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

    public void AssignPlayer(GameObject playerObject, LogCollisions collisionLoggerComponent)
    {
        player = playerObject;
        collisionLogger = collisionLoggerComponent;
    }

    public void ResetWall()
    {
        collisionDuration = 0;
        collisionEnterTime = 0;
    }

}
