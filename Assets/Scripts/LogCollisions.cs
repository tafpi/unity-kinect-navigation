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

    private ObstacleTrigger lastTrigger;

    private void Start()
    {
        
    }

    void OnDestroy()
    {
        CloseFile();
    }

    public void SetWalls(GameObject player)
    {
        foreach (var wallGroup in wallGroups)
        {
            if (wallGroup)
            {
                foreach (Transform wall in wallGroup.transform)
                {
                    ObstacleTrigger trigger = wall.gameObject.GetComponentInChildren<ObstacleTrigger>();

                    // assign each limit wall the player property
                    trigger.AssignPlayer(player, this);

                    // set trigger size by padding
                    Vector3 wallScale = wall.localScale;
                    trigger.gameObject.transform.localScale = new Vector3(1 + triggerPadding / wallScale.x, 1 + triggerPadding / wallScale.y, 1 + triggerPadding / wallScale.z);

                    // do not render the wall
                    wall.gameObject.GetComponent<MeshRenderer>().enabled = false;

                }
            }

        }
    }

    public void CreateFile(string path, string suffixFormat)
    {
        // create a file incrementing the filename's indexing
        string filename;
        fileCount = 0;
        do
        {
            fileCount++;
            filename = path + "/" + filenamePrefix + (fileCount > 0 ? "_" + fileCount.ToString(suffixFormat) : "") + ".csv";
        } while (File.Exists(filename));

        logFile = File.AppendText(filename);
        string headers = "Index, Type, Name, Location, Start Time (hh:mm:ss:fff), Duration (hh:mm:ss:fff)";
        logFile.WriteLine(headers);

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
