using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    // input
    public int userId;
    public GameObject player;

    // player management
    private PlayerMoveByKeyboard playerMoveByKeyboard;
    private PlayerMove playerMove;

    // file management
    private LogCollisions collisionsLogger;
    private LogPath pathLogger;
    private LogRun runLogger;
    public string directory = "UserTesting";
    private string filenameSuffixFormat = "0000.##";
    private string delimiter = ", ";

    // init vars
    [HideInInspector] public string colFilename;
    [HideInInspector] public string pathFilename;

    void Start()
    {
        playerMoveByKeyboard = player.GetComponent<PlayerMoveByKeyboard>();
        playerMove = player.GetComponent<PlayerMove>();
        collisionsLogger = GetComponent<LogCollisions>();
        pathLogger = GetComponent<LogPath>();
        runLogger = GetComponent<LogRun>();

        StartLogging();        
    }
    
    void Update()
    {
        if (pathLogger)
            pathLogger.OnUpdate(player);
    }

    void OnDestroy()
    {
        StopLogging();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
            // player reached finish
            runLogger.finishReached = true;
            StopLogging();
            player.GetComponent<PlayerManager>().canMove = false;
        }
    }

    private void StartLogging()
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        if (collisionsLogger)
            collisionsLogger.StartLogging(directory, filenameSuffixFormat);
        if (pathLogger)
            pathLogger.StartLogging(directory, filenameSuffixFormat);
        if (runLogger)
            runLogger.StartLogging(directory);
    }

    private void StopLogging()
    {
        if (collisionsLogger)
            collisionsLogger.StopLogging();
        if (pathLogger)
            pathLogger.StopLogging();
        if (runLogger)
            runLogger.StopLogging();
    }
}
