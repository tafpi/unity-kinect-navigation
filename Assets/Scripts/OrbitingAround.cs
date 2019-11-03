using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingAround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        // Spin the object around the world origin at 20 degrees/second.
        transform.RotateAround(transform.parent.position, transform.parent.up, 120 * Time.deltaTime);
    }
}
