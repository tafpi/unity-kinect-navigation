using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // ObstacleType is for collision logging reasons only.
    public enum ObstacleType { Wall, Fence, Prop };
    public ObstacleType obstacleType;
    public string location;
}
