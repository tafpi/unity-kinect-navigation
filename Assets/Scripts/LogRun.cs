using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LogRun : MonoBehaviour
{
    // Attach as component to finish object

    // input
    public bool isFirstRun;

    // computed at start
    public int runId;
    public GameObject gestureTravel;
    public GameObject gestureRotateY;
    public GameObject gestureRotateX;
    public System.DateTime startTime;
    public int totalPickups;

    // computed on update
    public float totalDuration;
    public float travelDuration;
    public float rotateYDuration;
    public float rotateXDuration;
    public float rotateWhileTravelDuration;
    public bool finishReached;
    public float idleControllerDuration;
    public float idleUserDuration;
    public int timesStopped;
    public float distanceTraveled;
    public int pathProximity;
    public int totalCollisions;
    public int wallCollisions;
    public int fenceCollisions;
    public int propCollisions;
    public int pickups;
    public float pickupSearch;

    // computed on end
    public System.DateTime closeTime;

    // file management
    public bool canLog;
    public string filename = "RunsFile.csv";
    private string delimiter = ", ";
    private int fileCount;
    private string fileContent;
    private StreamWriter logFile;
    private string path;
    private string line;
    private string header =
        "run id, " +
        "is first run, " +
        "gesture travel, " +
        "gesture rotate y, " +
        "gesture rotate x, " +
        "start time, " +
        "close time, " +
        "finish reached, " +
        "total duration, " +
        "travel duration, " +
        "rotate y duration, " +
        "rotate x duration, " +
        "rotate while travel duration, " +  // controller rotating (Y or X) while travelling
        "idle controller duration, " +      // controller not travelling and not rotating
        "idle user duration, " +
        "times stopped, " +
        "distance traveled, " +
        "path proximity %, " +
        "total collisions, " +
        "wall collisions, " +
        "fence collisions, " +
        "prop collisions, " +
        "pickups, " +
        "total pickups, " +
        "pickup search";

    // logging
    private int interval = 0;
    [Range(10, 30)] public int intervalDelay = 20;

    // other
    private LogSystem logSystem;
    private PickupPlayer pickupPlayer;


    private void Start()
    {        
        
    }

    private void Update()
    {
        totalDuration = Time.realtimeSinceStartup;

        if (interval > intervalDelay)
        {
            if (pickupPlayer)
            {
                pickups = pickupPlayer.picked;
                pickupSearch = pickupPlayer.totalTimeSearching;
            }

            if (logSystem)
            {
                //if (logSystem.player.)
                //{

                //}
                //travelDuration += 

            }

            interval = 0;
        }
        else
        {
            interval++;
        }

    }

    private int RunId(StreamReader r)
    {
        string lastLine = "";
        int linesCount = 0;
        while (true)
        {
            string line = r.ReadLine();
            if (line == null)
            {
                break;
            }
            else
            {
                lastLine = line;
                linesCount++;
            }
        }
        if (linesCount != 1)
        {
            int i;
            if (!int.TryParse(lastLine.Split(',')[0], out i))
            {
                i = -1;
            }
            return i + 1;
        }
        else
        {
            return 1;
        }
    }


    // methods used externally

    public void UpdateDistanceTraveled(float step)
    {
        distanceTraveled += step;        
    }

    public void StartLogging(string directory)
    {
        logSystem = GetComponent<LogSystem>();
        path = directory + "/" + filename;

        if (!File.Exists(path))
        {
            using (StreamWriter writer = WriteStream(path))
            {
                if (writer is null)
                    return;
                canLog = true;
                writer.WriteLine(header);
            }
        }

        using (StreamReader reader = ReadStream(path))
        {
            if (reader is null)
                return;
            canLog = true;

            // logs on start
            runId = RunId(reader);

            PlayerMove playerMove = logSystem.player.GetComponent<PlayerMove>();
            if (playerMove)
                gestureTravel = playerMove.travelGesture;
            PlayerLookLeftRight playerLookY = logSystem.GetComponent<PlayerLookLeftRight>();
            if (playerLookY)
                gestureRotateY = playerLookY.rotateGesture;
            PlayerLookUpDown playerLookX = logSystem.GetComponentInChildren<PlayerLookUpDown>();
            if (playerLookX)
                gestureRotateX = playerLookX.rotateGesture;

            startTime = System.DateTime.Now;

            pickupPlayer = logSystem.player.GetComponentInChildren<PickupPlayer>();
            if (pickupPlayer)
                totalPickups = pickupPlayer.pickups.Length;

        }

    }

    public void StopLogging()
    {
        using (StreamWriter writer = WriteStream(path))
        {
            if (writer is null)
                return;
            
            closeTime = System.DateTime.Now;

            line =
                runId + delimiter +
                isFirstRun + delimiter +
                gestureTravel + delimiter +
                gestureRotateY + delimiter +
                gestureRotateX + delimiter +
                startTime + delimiter +
                closeTime + delimiter +
                finishReached + delimiter +
                totalDuration + delimiter +
                travelDuration + delimiter +
                rotateYDuration + delimiter +
                rotateXDuration + delimiter +
                rotateWhileTravelDuration + delimiter +
                idleControllerDuration + delimiter +
                idleUserDuration + delimiter +
                timesStopped + delimiter +
                distanceTraveled + delimiter +
                pathProximity + delimiter +
                totalCollisions + delimiter +
                wallCollisions + delimiter +
                fenceCollisions + delimiter +
                propCollisions + delimiter +
                pickups + delimiter +
                totalPickups + delimiter +
                pickupSearch;

            writer.WriteLine(line);
            writer.Close();
        }
        
    }

    static StreamWriter WriteStream(string path)
    {
        if (path is null)
        {
            Debug.Log("You did not supply a file path.");
            //canLog = false;
            return null;
        }

        try
        {
            var fs = new FileStream(path, FileMode.Append);
            //canLog = true;
            return new StreamWriter(fs);
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
        {
            //canLog = false;
            Debug.Log("There is a sharing violation.");
        }
        catch (IOException e)
        {
            Debug.Log($"An exception occurred:\nError code: " +
                              $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");

        }
        return null;
    }

    static StreamReader ReadStream(string path)
    {
        if (path is null)
        {
            Debug.Log("You did not supply a file path.");
            //canLog = false;
            return null;
        }

        try
        {
            var fs = new FileStream(path, FileMode.Open);
            //canLog = true;
            return new StreamReader(fs);
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
        {
            //canLog = false;
            Debug.Log("There is a sharing violation.");
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Debug.Log("SETUP ERROR: RunsFile.csv is open. Close file and rerun.");
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
        {
            //canLog = true;
            Debug.Log("The file already exists.");
        }
        catch (IOException e)
        {
            Debug.Log($"An exception occurred:\nError code: " +
                              $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");

        }
        return null;
    }

}
