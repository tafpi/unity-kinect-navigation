using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PickupPlayer : MonoBehaviour
{
    public GameObject[] pickupGroups;
    public int interactionTimeNeeded = 2;
    [Range(0f, 1f)] public float viewportFactor = 0.35f;
    public GameObject player;
    public Camera cam;
    [HideInInspector] public int picked;
    [HideInInspector] public float totalTimeSearching;

    private void Awake()
    {
        cam = GetComponentInParent<Camera>();
        player = cam.transform.parent.gameObject;        
        foreach (var pickupGroup in pickupGroups)
        {
            foreach (Transform pickup in pickupGroup.transform)
            {
                PickupObject pickupObject = pickup.GetComponent<PickupObject>();
                pickupObject.pickupPlayer = this;
            }
        }
    }

    void Start()
    {

    }

    public bool PickupInTarget(Vector3 pickupPos)
    {
        // check if pickup object's position relative to camera viewport is in bounds of camera's target
        Vector3 viewPos = cam.WorldToViewportPoint(pickupPos);
        return (viewPos.x > (1 - viewportFactor) / 2 && viewPos.x < (1 + viewportFactor) / 2 && viewPos.y > (1 - viewportFactor) / 2 && viewPos.y < (1 + viewportFactor) / 2);
    }

    private void OnDrawGizmos()
    {
        // draw a target: part of the viewport with same aspect ratio and dimensions defined by factoring. ie: factor 1 means same dimensions as viewport
        Camera c = GetComponentInParent<Camera>();
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(c.transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, c.fieldOfView * viewportFactor, 10f, 10.5f, c.aspect);
    }
}