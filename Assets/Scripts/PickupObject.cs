using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //[HideInInspector]
    private float interactingDuration;
    private float interactionInitTime;
    public Material successMaterial;
    public PickupPlayer pickupPlayer;
    public GameObject pickupTarget;
    public bool playerInTrigger = false;
    private bool picked = false;
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
            if (searchDuration == 0)
                timeInTrigger = Time.realtimeSinceStartup;
            searchDuration = Time.realtimeSinceStartup - timeInTrigger;
            
            Vector3 viewPos = pickupPlayer.cam.WorldToViewportPoint(transform.position);
            if (pickupPlayer.PickupInTarget(pickupTarget.transform.position))
            {
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
                interactionInitTime = 0;
            }

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

}