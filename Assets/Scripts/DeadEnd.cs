using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnd : MonoBehaviour
{
    public GameObject pavementPrev;
    public GameObject pavementNext;
    public GameObject blockingFence;
    public GameObject blockingCollider;
    [HideInInspector] public GameObject player;

    private GameObject text;

    private void Awake()
    {
        text = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        pavementNext.SetActive(false);
        text.SetActive(false);
        blockingFence.SetActive(true);
        blockingCollider.SetActive(true);
        blockingCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ReferenceEquals(other.gameObject, player))
        {
            pavementPrev.SetActive(false);
            pavementNext.SetActive(true);
            text.SetActive(true);
            blockingFence.SetActive(false);
            blockingCollider.SetActive(false);
        }
    }
}
