using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    // input
    public GameObject player;
    public int userIdIndex;
    public enum GestureSet { GS1, GS2 };
    public GestureSet gestureSet;
    public enum Round { RND1, RND2 };
    public Round round;

    // player management
    private PlayerMoveByKeyboard playerMoveByKeyboard;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public PathProximity pathProximity;

    // file management
    public LogCollisions collisionsLogger;
    public LogPath pathLogger;
    public LogRun runLogger;
    public string directory = "UserTesting";
    public string runId;
    private string userIdPrefix = "U";
    public string userId;
    private string countSuffixFormat = "000.##";
    //private string delimiter = ", ";

    // init vars
    [HideInInspector] public string colFilename;
    [HideInInspector] public string pathFilename;

    // other
    private int interval = 0;
    private int intervalDelay = 20;

    void Start()
    {
        userId = userIdPrefix + userIdIndex.ToString(countSuffixFormat);
        StartLogging();
    }
    
    void Update()
    {
        if (interval > intervalDelay)
        {
            if (pathLogger)
                pathLogger.OnUpdate(this);
            

            //proximityPercentage = logSystem.pathProximity.proximityPercentage;

            interval = 0;
        }
        else
        {
            interval++;
        }
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
            //PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.canMove = false;
            playerManager.travelling = false;
            if (runLogger.logging)
                StopLogging();
        }
    }

    private void StartLogging()
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        if (runLogger)
        {
            runLogger.StartLogging(this);
            runId = runLogger.RunId(this, countSuffixFormat);
        }
        if (collisionsLogger)
            collisionsLogger.StartLogging(this);
        if (pathLogger)
            pathLogger.StartLogging(this);

        //playerMoveByKeyboard = player.GetComponent<PlayerMoveByKeyboard>();
        //playerMove = player.GetComponent<PlayerMove>();
        //pathProximity = player.GetComponent<PathProximity>();
        //playerManager = player.GetComponent<PlayerManager>();
        //playerManager.logRun = runLogger;
    }

    private void StopLogging()
    {
        Debug.Log("System Stop Logging");
        if (runLogger)
        {
            runLogger.StopLogging(this);
        }
        if (collisionsLogger)
            collisionsLogger.StopLogging();
        if (pathLogger)
        {

            pathLogger.StopLogging(this);
        }
    }

    public void AbortLog(string filename)
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Debug.Log("SETUP ERROR: " + filename + " is open. Close file and rerun.");
            pathLogger.canLog = false;
            runLogger.canLog = false;
            collisionsLogger.StopLogging();
            collisionsLogger.DeleteLastLog();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
