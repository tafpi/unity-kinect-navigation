using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //[HideInInspector]
    public Material successMaterial;
    public PickupPlayer pickupPlayer;
    public GameObject pickupTarget;
    public bool playerInTrigger = false;
    private bool picked = false;
    [HideInInspector] public float interactionInitTime;
    [HideInInspector] public float interactingDuration;
    [HideInInspector] public float searchDuration;
    [HideInInspector] public float timeInTrigger;
    
    private void Start()
    {
        interactionInitTime = 0;
    }

    private void Update()
    {
        if (playerInTrigger && !picked)
        {
            pickupPlayer.drawTarget = true;
            pickupPlayer.targetColor = Color.red;
            if (searchDuration == 0)
                timeInTrigger = Time.realtimeSinceStartup;
            searchDuration = Time.realtimeSinceStartup - timeInTrigger;

            Vector3 viewPos = pickupPlayer.cam.WorldToViewportPoint(transform.position);
            if (pickupPlayer.PickupInTarget(pickupTarget.transform.position, pickupTarget.transform.GetChild(0).GetComponent<Renderer>()))
            {
                pickupPlayer.targetColor = Color.green;
                if (interactionInitTime == 0)
                    interactionInitTime = Time.realtimeSinceStartup;
                interactingDuration = Time.realtimeSinceStartup - interactionInitTime;

                if (interactingDuration > pickupPlayer.interactionTimeNeeded)
                {
                    Pick();
                }
            }
            else
            {
                interactionInitTime = Time.realtimeSinceStartup;
            }

        }
        else
        {
            interactionInitTime = Time.realtimeSinceStartup;
            pickupPlayer.drawTarget = false;
        }
    }

    public void Pick()
    {
        picked = true;
        Renderer[] renderers = pickupTarget.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = successMaterial;
        }
        if (pickupPlayer)
        {
            pickupPlayer.picked++;
            pickupPlayer.totalTimeSearching += searchDuration;
        }
    }

    private void OnDrawGizmos()
    {
        if (pickupPlayer)
        {
            if (pickupPlayer.drawTarget)
            {
                string message = pickupPlayer.targetColor == Color.green ? ""+(pickupPlayer.interactionTimeNeeded - interactingDuration) : "look at object for 2 seconds";
                Handles.Label(transform.position, message);
            }
        }
    }

}