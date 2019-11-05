using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierSolution
{
    [ExecuteInEditMode]
    public class RepeatAlongBezier : MonoBehaviour
    {
        public BezierSpline spline;
        public GameObject deviationCollider;
        public GameObject placeholder;

        bool refreshing = false;
        int n = 5;
        GameObject collidersParent;

        private void OnEnable()
        {
            collidersParent = Instantiate(placeholder, transform);
            for (int i = 0; i < n; i++)
            {
                Instantiate(deviationCollider, spline.GetPoint((float)i / (n - 1)), deviationCollider.transform.rotation, collidersParent.transform);
                //if (i == 4)
                //{
                //    refreshing = false;
                //}
            }
        }

        private void OnDisable()
        {
            DestroyImmediate(collidersParent);
            //foreach (Transform child in placeholder.transform)
            //{
            //    GameObject.DestroyImmediate(child.gameObject);
            //}
        }

        void Refresh()
        {
            if (placeholder.transform.childCount == n)
            {
                foreach (Transform child in placeholder.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            if (placeholder.transform.childCount == 0)
            {
                //refreshing = true;
                for (int i = 0; i < n; i++)
                {
                    Instantiate(deviationCollider, spline.GetPoint((float)i / (n-1)), deviationCollider.transform.rotation, placeholder.transform);
                    //if (i == 4)
                    //{
                    //    refreshing = false;
                    //}
                }
            }
        }
        
    }

}
