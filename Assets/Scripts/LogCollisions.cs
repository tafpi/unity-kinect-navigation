using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogCollisions : MonoBehaviour
{
    // Attach as component to player.
    
    // input
    public string filenamePrefix = "CollisionsFile";
    public GameObject[] wallGroups;
    public float triggerPadding = 1;

    private StreamWriter logFile;
    private string delimiter = ", ";
    private int fileCount;
    private int collisionIndex = 0;
    private string filename;

    private ObstacleTrigger lastTrigger;
    //public LogSystem logSystem;
    
    private void Start()
    {
        
    }

    public void StartLogging(LogSystem logSystem)
    {
        // get log system
        //logSystem = GetComponent<LogSystem>();

        // set each obstacle's trigger and player object
        foreach (var wallGroup in wallGroups)
        {
            if (wallGroup)
            {
                foreach (Transform wall in wallGroup.transform)
                {
                    ObstacleTrigger trigger = wall.gameObject.GetComponentInChildren<ObstacleTrigger>();

                    // assign each limit wall the player property
                    trigger.AssignProperties(logSystem.player, this, logSystem);

                    // set trigger size by padding
                    Vector3 wallScale = wall.localScale;
                    trigger.gameObject.transform.localScale = new Vector3(1 + triggerPadding / wallScale.x, 1 + triggerPadding / wallScale.y, 1 + triggerPadding / wallScale.z);

                    // do not render the wall
                    wall.gameObject.GetComponent<MeshRenderer>().enabled = false;

                }
            }

        }

        // create a file incrementing the filename's indexing
        string[] files = System.IO.Directory.GetFiles(logSystem.directory, filenamePrefix + "*.csv", System.IO.SearchOption.TopDirectoryOnly);
        if(int.TryParse(logSystem.runId.Split('R')[1], out int i) && i-1 != files.Length)
        {
            logSystem.AbortLog("LOGGING ERROR: " + filename + " files and rows are mucked up.");
            return;
        }

        filename = logSystem.directory + "/" + filenamePrefix + "-" + logSystem.filenameLabel + ".csv";

        logFile = File.AppendText(filename);
        string headers = "Index, Type, Name, Location, Start Time (hh:mm:ss:fff), Duration (hh:mm:ss:fff)";
        logFile.WriteLine(headers);
    }

    public void StopLogging()
    {
        // close the file if there is one
        if (logFile != null)
        {
            if (logFile.BaseStream != null)
            {
                CollisionEndHandler(lastTrigger);
                logFile.Close();
            }
        }
    }

    public void DeleteLastLog()
    {
        if (filename != null)
            File.Delete(filename);
    }

    public void CollisionBegin(ObstacleTrigger obstacleTrigger, LogSystem logSystem)
    {
        //Debug.Log("collision begin");
        obstacleTrigger.collisionEnterTime = Time.realtimeSinceStartup;

        Obstacle obstacle = obstacleTrigger.GetComponentInParent<Obstacle>();
        logSystem.runLogger.totalCollisions++;
        if (obstacle.obstacleType == Obstacle.ObstacleType.Wall)
            logSystem.runLogger.wallCollisions++;
        if (obstacle.obstacleType == Obstacle.ObstacleType.Fence)
            logSystem.runLogger.fenceCollisions++;
        if (obstacle.obstacleType == Obstacle.ObstacleType.Prop)
            logSystem.runLogger.propCollisions++;
        if (obstacle.obstacleType == Obstacle.ObstacleType.RoadBlock)
            logSystem.runLogger.roadblockCollisions++;
        if (obstacle.obstacleType == Obstacle.ObstacleType.Invisible)
            logSystem.runLogger.invisibleCollisions++;
    }

    public void CollisionUpdate(ObstacleTrigger obstacleTrigger)
    {
        //Debug.Log("collision update");
        obstacleTrigger.collisionDuration = Time.realtimeSinceStartup - obstacleTrigger.collisionEnterTime;
        lastTrigger = obstacleTrigger;
    }

    public void CollisionEnd(ObstacleTrigger obstacleTrigger)
    {
        CollisionEndHandler(obstacleTrigger);
    }

    public void CollisionEndHandler(ObstacleTrigger obstacleTrigger)
    {
        if (obstacleTrigger)
        {
            if (obstacleTrigger.collisionEnterTime > 0)
            {
                Obstacle obstacle = obstacleTrigger.GetComponentInParent<Obstacle>();
                string collisionLine = "";
                collisionLine += collisionIndex;
                collisionLine += delimiter;
                collisionLine += obstacle.obstacleType;
                collisionLine += delimiter;
                collisionLine += obstacle.name;
                collisionLine += delimiter;
                collisionLine += obstacle.location;
                collisionLine += delimiter;
                collisionLine += TimeFormat(obstacleTrigger.collisionEnterTime);
                collisionLine += delimiter;
                collisionLine += TimeFormat(obstacleTrigger.collisionDuration);
                if (logFile != null)
                {
                    logFile.WriteLine(collisionLine);
                }
                collisionIndex++;
                obstacleTrigger.ResetWall();
            }
        }
    }

    private string TimeFormat(float seconds)
    {
        // accepts seconds, returns formatted timestamp
        System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);
        return time.ToString("hh':'mm':'ss':'fff");
    }
    
}
