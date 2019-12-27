using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTarget : MonoBehaviour
{
    void Start()
    {
        GetComponentInParent<PickupObject>().pickupTarget = gameObject;
    }
}