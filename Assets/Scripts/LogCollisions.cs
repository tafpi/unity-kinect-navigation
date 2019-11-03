using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogCollisions : MonoBehaviour
{
    // Attach as component to player.

    private string directoryName = "UserTesting";
    private string fileName = "collisionsFile";
    private StreamWriter logFile;
    private string fmt = "0000.##";
    private string delimiter = ", ";
    private int fileCount;

    public GameObject[] walls;
    public float triggerPadding = 1;

    private int collisionIndex = 0;

    private ObstacleTrigger lastTrigger;

    //Travel Gesture, Rotate Y Gesture, Rotate X Gesture
    //TravelGesture_RotateYGesture_RotateXGesture
    
    void Start()
    {
        // if directory doesn't exit, create it
        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        // create a file incrementing the filename's indexing
        fileCount = 0;
        do
        {
            fileCount++;
        } while (File.Exists(directoryName + "/" + fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".csv"));

        logFile = File.AppendText(directoryName + "/" + fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".csv");
        string headers = "Index, Type, Name, Location, Start Time (hh:mm:ss:fff), Duration (hh:mm:ss:fff)";
        logFile.WriteLine(headers);
        
        foreach (var wall in walls)
        {
            if (wall)
            {
                ObstacleTrigger trigger = wall.GetComponentInChildren<ObstacleTrigger>();

                // assign each limit wall the player property
                trigger.AssignPlayer(gameObject);

                // set trigger size by padding
                Vector3 wallScale = wall.transform.localScale;
                trigger.gameObject.transform.localScale = new Vector3(1 + triggerPadding / wallScale.x, 1 + triggerPadding / wallScale.y, 1 + triggerPadding / wallScale.z);

                // do not render the wall
                wall.GetComponent<MeshRenderer>().enabled = false;
            }

        }
    }

    void OnDestroy()
    {
        CloseFile();
    }
    
    public void CollisionBegin(ObstacleTrigger obstacleTrigger)
    {
        //Debug.Log("collision begin");
        obstacleTrigger.collisionEnterTime = Time.realtimeSinceStartup;
    }

    public void CollisionUpdate(ObstacleTrigger obstacleTrigger)
    {
        //Debug.Log("collision update");
        obstacleTrigger.collisionDuration = Time.realtimeSinceStartup - obstacleTrigger.collisionEnterTime;
        lastTrigger = obstacleTrigger;
    }

    public void CollisionEnd(ObstacleTrigger obstacleTrigger)
    {
        //Debug.Log("collision end");
        CollisionEndHandler(obstacleTrigger);
    }

    public void OnApplicationQuit()
    {
        CollisionEndHandler(lastTrigger);
    }

    public void CloseFile()
    {
        // close the file if there is one
        if (logFile != null)
        {
            if (logFile.BaseStream != null)
            {
                logFile.Close();
            }
        }
    }

    public void CollisionEndHandler(ObstacleTrigger obstacleTrigger)
    {
        if (obstacleTrigger)
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

    private string TimeFormat(float seconds)
    {
        // accepts seconds, returns formatted timestamp
        System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);
        return time.ToString("hh':'mm':'ss':'fff");
    }
    
}
