using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierSolution
{
    public class DistanceFromPath : MonoBehaviour
    {
        public BezierSpline path;
        public float deviation;

        // Update is called once per frame
        void Update()
        {
            Vector3 nearestPoint = path.FindNearestPointTo(transform.position);
            nearestPoint.y = transform.position.y;
            deviation = Vector3.Distance(nearestPoint, transform.position);
        }
    }
}
