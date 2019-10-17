using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRun : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject);
        Debug.Log(player);
        if (ReferenceEquals(collider.gameObject, player))
        {
            player.GetComponent<PlayerMoveByKeyboard>().canMove = false;
            player.GetComponent<LogCollisions>().CloseFile();
            player.GetComponent<LogPath>().CloseFile();
        }
    }
}
