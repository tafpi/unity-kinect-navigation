using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogCollisions : MonoBehaviour
{
    // attach as component to player

    public bool keepTrack;
    private string fileName = "collisionsFile";
    private StreamWriter logFile;
    private string fmt = "0000.##";
    private string delimiter = ", ";
    public int fileCount;

    public GameObject[] walls;
    public float triggerPadding = 10;

    private int collisionIndex = 0;

    //Travel Gesture, Rotate Y Gesture, Rotate X Gesture
    //TravelGesture_RotateYGesture_RotateXGesture
    
    void Start()
    {
        // create a file incrementing the filename's indexing
        fileCount = 0;
        do
        {
            fileCount++;
        } while (File.Exists(fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".csv"));

        if (keepTrack)
        {
            logFile = File.AppendText(fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".csv");
            string headers = "Index, Wall Name, Start Time (hh:mm:ss:fff), Duration (hh:mm:ss:fff)";
            logFile.WriteLine(headers);
        }

        foreach (var wall in walls)
        {
            LimitWallTrigger trigger = wall.GetComponentInChildren<LimitWallTrigger>();

            // assign each limit wall the player property
            trigger.AssignPlayer(gameObject);

            // set trigger size by padding
            Vector3 wallScale = wall.transform.localScale;
            trigger.gameObject.transform.localScale = new Vector3(1 + triggerPadding / wallScale.x, 1 + triggerPadding / wallScale.y, 1 + triggerPadding / wallScale.z);

        }
    }

    void OnDestroy()
    {
        if (logFile != null)
        {
            logFile.Close();
        }
    }
    
    public void CollisionBegin(LimitWallTrigger wallTrigger)
    {
        //Debug.Log("collision begin");
        wallTrigger.collisionEnterTime = Time.realtimeSinceStartup;
    }

    public void CollisionUpdate(LimitWallTrigger wallTrigger)
    {
        //Debug.Log("collision update");
        wallTrigger.collisionDuration = Time.realtimeSinceStartup - wallTrigger.collisionEnterTime;
    }

    public void CollisionEnd(LimitWallTrigger wallTrigger)
    {
        //Debug.Log("collision end");
        string collisionLine = "";
        collisionLine += collisionIndex;
        collisionLine += delimiter;
        collisionLine += wallTrigger.gameObject.transform.parent.name;
        collisionLine += delimiter;
        collisionLine += TimeFormat(wallTrigger.collisionEnterTime);
        collisionLine += delimiter;
        collisionLine += TimeFormat(wallTrigger.collisionDuration);
        if (logFile != null)
        {
            logFile.WriteLine(collisionLine);
        }
        collisionIndex++;
        wallTrigger.ResetWall();
    }

    private string TimeFormat(float seconds)
    {
        // takes in seconds, returns formatted timestamp
        System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);
        return time.ToString("hh':'mm':'ss':'fff");
    }

    private void SetWallTriggerScale()
    {
        foreach (var wall in walls)
        {
            
        }
    }

}
