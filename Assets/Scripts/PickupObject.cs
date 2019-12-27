using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //[HideInInspector]
    public Material successMaterial;
    [HideInInspector] public PickupPlayer pickupPlayer;
    [HideInInspector] public GameObject pickupTarget;
    [HideInInspector] public bool playerInTrigger = false;
    [HideInInspector] public float interactionInitTime;
    [HideInInspector] public float interactingDuration;
    [HideInInspector] public float searchDuration;
    [HideInInspector] public float timeInTrigger;

    private GUIStyle centeredStyle;
    private bool picked = false;

    private void Start()
    {
        interactionInitTime = 0;
    }

    private void Update()
    {
        if (playerInTrigger && !picked)
        {
            pickupPlayer.targetColor = Color.red;
            if (searchDuration == 0)
                timeInTrigger = Time.realtimeSinceStartup;
            searchDuration = Time.realtimeSinceStartup - timeInTrigger;
            
            Vector3 viewPos = pickupPlayer.cam.WorldToViewportPoint(transform.position);
            if (pickupPlayer.PickupInTarget(pickupTarget.transform.position, pickupPlayer.activePickupObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>()))
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

            pickupPlayer.TotalSearchTime();
        }
        else
        {
            interactionInitTime = 0;            
        }
    }

    public void Pick()
    {
        picked = true;
        pickupPlayer.canvas.gameObject.SetActive(false);
        Renderer[] renderers = pickupTarget.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = successMaterial;
        }
        if (pickupPlayer)
        {
            pickupPlayer.picked++;
            pickupPlayer.logSystem.runLogger.pickups++;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
    }

    private void OnGUI()
    {
        if (pickupPlayer)
        {
            if (ReferenceEquals(pickupPlayer.activePickupObject, this))
            {
                //if (pickupPlayer.drawTarget)
                if (pickupPlayer.canvas.isActiveAndEnabled)
                {
                    centeredStyle = GUI.skin.GetStyle("Label");
                    centeredStyle.alignment = TextAnchor.UpperCenter;
                    centeredStyle.fontSize = 18;
                    string message = "look at object for 2 seconds";
                    centeredStyle.normal.textColor = pickupPlayer.targetColor;
                    if (pickupPlayer.targetColor == Color.green)
                        message = ""+(Mathf.Round((pickupPlayer.interactionTimeNeeded - interactingDuration)*10)/10).ToString("0.0#");

                    GUI.Label(new Rect(0, Screen.height/4, Screen.width, Screen.height), message, centeredStyle);
                }
            }
        }        
    }

}