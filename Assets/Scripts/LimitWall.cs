using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWall : MonoBehaviour
{
    public bool playerIsColliding;
    public bool playerWasColliding;
    public float collisionDuration;
    public float collisionEnterTime;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetWall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void ResetWall()
    {
        playerIsColliding = false;
        playerWasColliding = false;
        collisionDuration = 0;
        collisionEnterTime = 0;
    }
    
}
