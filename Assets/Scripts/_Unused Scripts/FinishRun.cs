using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRun : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject player;
    private PlayerMoveByKeyboard playerMoveByKeyboard;
    private PlayerMove playerMove;

    private void Start()
    {
        playerMoveByKeyboard = player.GetComponent<PlayerMoveByKeyboard>();
        playerMove = player.GetComponent<PlayerMove>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject);
        Debug.Log(player);
        if (ReferenceEquals(collider.gameObject, player))
        {
            playerManager.canMove = false;
        }
    }
}
