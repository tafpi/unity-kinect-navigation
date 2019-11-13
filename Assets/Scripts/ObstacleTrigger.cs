using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{

    [HideInInspector] public float collisionDuration;
    [HideInInspector] public float collisionEnterTime;
    public GameObject player;
    private LogSystem logSystem;
    private LogCollisions collisionLogger;
    
    void Start()
    {
        ResetWall();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
            collisionLogger.CollisionBegin(this, logSystem);
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

    public void AssignProperties(GameObject playerObject, LogCollisions collisionLoggerComponent, LogSystem logSystemComponent)
    {
        player = playerObject;
        collisionLogger = collisionLoggerComponent;
        logSystem = logSystemComponent;
    }

    public void ResetWall()
    {
        collisionDuration = 0;
        collisionEnterTime = 0;
    }

}
