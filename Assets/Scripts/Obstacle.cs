using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // ObstacleType is for collision logging reasons only.
    public enum ObstacleType { Wall, Fence, Prop, RoadBlock, Invisible };
    public ObstacleType obstacleType;
    public enum ObstacleLocation { RightSide, LeftSide, Other };
    public ObstacleLocation location;
}
