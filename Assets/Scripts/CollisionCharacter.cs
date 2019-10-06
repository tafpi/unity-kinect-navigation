using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCharacter : MonoBehaviour
{
    //private CharacterController controller;
    private LogCollisions collisionLogger;
    //public bool isColliding;
    //public GameObject[] limitWalls;
    //private LimitWallCollision[] collisions;

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        collisionLogger = GetComponent<LogCollisions>();
    }

    void Update()
    {
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject wallObject = hit.collider.gameObject;
        collisionLogger.UpdateCollisions(wallObject);

        //LimitWall wall = wallObject.GetComponent<LimitWall>();
        //bool anyCollisions = false;
        //foreach (var item in limitWalls)
        //{
        //    if (GameObject.ReferenceEquals(item, wallObject))
        //    {
        //        anyCollisions = true;
        //        isColliding = true;
        //        if (collisionLogger)
        //        {
        //            Debug.Log(collisionLogger);
        //            collisionLogger.logCollision(wallObject);
        //        }
        //    }
        //}
    }

    void FixedUpdate()
    {
        //if (isColliding)
        //{
        //    if ((controller.collisionFlags & CollisionFlags.Sides) == 0)
        //    {
        //        isColliding = false;
        //        Debug.Log("exit every collision");
        //    }
        //}
    }

    

}
