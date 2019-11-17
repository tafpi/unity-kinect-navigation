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
            {
                pickupObject.playerInTrigger = true;
                //pickupObject.pickupPlayer.drawTarget = true;
                pickupObject.pickupPlayer.canvas.gameObject.SetActive(true);
                pickupObject.pickupPlayer.activePickupObject = pickupObject;
            }
                
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickupObject.pickupPlayer)
            if (ReferenceEquals(pickupObject.pickupPlayer.player, other.gameObject))
            {
                pickupObject.playerInTrigger = false;
                //pickupObject.pickupPlayer.drawTarget = false;
                pickupObject.pickupPlayer.canvas.gameObject.SetActive(false);
                pickupObject.pickupPlayer.activePickupObject = null;
            }
    }

}
