using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    // input
    public int userId;
    public GameObject player;

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
        // if directory doesn't exit, create it
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        collisionsLogger = GetComponent<LogCollisions>();
        pathLogger = GetComponent<LogPath>();
        runLogger = GetComponent<LogRun>();

        if (pathLogger)
        {
            pathLogger.CreateFile(directory, filenameSuffixFormat);
        }

        if (collisionsLogger)
        {
            collisionsLogger.CreateFile(directory, filenameSuffixFormat);
            collisionsLogger.SetWalls(player);
        }
        
    }
    
    void Update()
    {
        if (pathLogger)
        {
            pathLogger.OnUpdate(player);
        }
        
    }

    void OnDestroy()
    {
        if (pathLogger)
        {
            pathLogger.CloseFile();
        }

        if (collisionsLogger)
        {
            collisionsLogger.CloseFile();

        }
    }
}
