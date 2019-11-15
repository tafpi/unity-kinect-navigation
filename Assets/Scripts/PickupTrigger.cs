using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    private PickupObject pickupObject;

    private void Start()
    {
        pickupObject = GetComponentInParent<PickupObject>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (pickupObject.pickupPlayer)
            if (ReferenceEquals(pickupObject.pickupPlayer.player, other.gameObject))
                pickupObject.playerInTrigger = true;
                
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickupObject.pickupPlayer)
            if (ReferenceEquals(pickupObject.pickupPlayer.player, other.gameObject))
                pickupObject.playerInTrigger = false;
    }

}
