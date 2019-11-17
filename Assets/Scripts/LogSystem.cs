using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    // input
    public GameObject player;
    public int userIdIndex;
    //public enum GestureSet { GS1, GS2 };
    [HideInInspector] public string gestureSetId;
    public enum Round { RND1, RND2 };
    public Round roundId;
    public GameObject pickupGroup;
    public GameObject deadEnds;
    public BezierSolution.BezierSpline path;

    // player management
    private PlayerMoveByKeyboard playerMoveByKeyboard;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public PickupPlayer pickupPlayer;

    // file management
    public LogCollisions collisionsLogger;
    public LogPath pathLogger;
    public LogRun runLogger;
    public PathProximity pathProximity;
    public string directory = "UserTesting";
    public string runId;
    private string userIdPrefix = "U";
    public string userId;
    private string countSuffixFormat = "000.##";
    public string filenameLabel;
    //private string delimiter = ", ";
    private bool logging;

    // init vars
    [HideInInspector] public string colFilename;
    [HideInInspector] public string pathFilename;

    // other
    private int interval = 0;
    private int intervalDelay = 20;

    private void Awake()
    {
        //player.SetActive(true);
        gestureSetId = "MK"; // mouse - keyboard
        if (player.name.Split('-').Length > 1)
            gestureSetId = player.name.Split('-')[1];
        playerMoveByKeyboard = player.GetComponent<PlayerMoveByKeyboard>();
        playerMove = player.GetComponent<PlayerMove>();
        playerManager = player.GetComponent<PlayerManager>();
        pickupPlayer = player.GetComponent<PickupPlayer>();

        foreach (Transform deadEndTransform in deadEnds.transform)
        {
            deadEndTransform.GetComponent<DeadEnd>().player = player;
        }

        // hide finish text
        transform.GetChild(1).gameObject.SetActive(false);
    }

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
        if (logging)
            StopLogging();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (ReferenceEquals(collider.gameObject, player))
        {
            // player reached finish
            runLogger.finishReached = true;
            playerManager.canMove = false;
            playerManager.travelling = false;

            // show finish text
            transform.GetChild(1).gameObject.SetActive(true);

            if (logging)
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
            filenameLabel = runId + "-" + userId + "-" + gestureSetId + "-" + roundId;
        }
        if (collisionsLogger)
            collisionsLogger.StartLogging(this);
        if (pathLogger)
            pathLogger.StartLogging(this);

        playerManager.logRun = runLogger;

        logging = true;
    }

    private void StopLogging()
    {
        if (runLogger)
            runLogger.StopLogging(this);
        if (collisionsLogger)
            collisionsLogger.StopLogging();
        if (pathLogger)
            pathLogger.StopLogging(this);
        logging = false;
    }

    public void AbortLog(string error)
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Debug.Log(error);
            pathLogger.canLog = false;
            runLogger.canLog = false;
            collisionsLogger.StopLogging();
            collisionsLogger.DeleteLastLog();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
