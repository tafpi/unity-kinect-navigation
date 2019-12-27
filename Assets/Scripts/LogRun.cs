using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LogRun : MonoBehaviour
{
    
    // computed at start
    public System.DateTime startTime;
    [HideInInspector] public int totalPickups;

    // computed on update
    [HideInInspector] public float totalDuration;
    [HideInInspector] public float totalTravelDuration;
    [HideInInspector] public float backwardsTravelDuration;
    [HideInInspector] public float rotateYDuration;
    [HideInInspector] public float rotateXDuration;
    [HideInInspector] public float rotateWhileTravelDuration;
    [HideInInspector] public bool finishReached;
    [HideInInspector] public float idleControllerDuration;
    [HideInInspector] public int timesStopped;
    [HideInInspector] public float distanceTraveled;
    [HideInInspector] public int pathProximityPercentage;
    [HideInInspector] public int totalCollisions;
    [HideInInspector] public int wallCollisions;
    [HideInInspector] public int fenceCollisions;
    [HideInInspector] public int propCollisions;
    [HideInInspector] public int roadblockCollisions;
    [HideInInspector] public int invisibleCollisions;
    [HideInInspector] public int pickups;
    [HideInInspector] public float pickupSearch;

    // computed on end
    [HideInInspector] public System.DateTime closeTime;

    // file management
    public string filename = "RunsLog.csv";
    public string runPrefix = "R";
    private string delimiter = ", ";
    private int fileCount;
    private string fileContent;
    private StreamWriter logFile;
    private string line;
    private string header =
        "run id, " +
        "user id, " +
        "gesture set, " +
        "round, " +
        "start time, " +
        "close time, " +
        "finish reached, " +
        "total duration, " +
        "total travel duration, " +
        "backwards travel duration, " +
        "rotate y duration, " +
        "rotate x duration, " +
        "rotate while travel duration, " +  // controller rotating (Y or X) while travelling
        "idle controller duration, " +      // controller not travelling and not rotating
        "times stopped, " +
        "distance traveled, " +
        "path proximity %, " +
        "total collisions, " +
        "wall collisions, " +
        "fence collisions, " +
        "prop collisions, " +
        "roadblock collisions, " +
        "invisible collisions, " +
        "pickups, " +
        "total pickups, " +
        "pickup search";

    // logging
    [HideInInspector] public bool canLog;
    [HideInInspector] public bool logging;
    private int interval = 0;
    [Range(10, 30)] public int intervalDelay = 20;

    private void Start()
    {        
        
    }

    private void Update()
    {
        totalDuration = Time.realtimeSinceStartup;
    }

    public string RunId(LogSystem logSystem, string countFormat)
    {
        string path = logSystem.directory + "/" + filename;
        using (StreamReader reader = ReadStream(path, logSystem))
        {
            string lastLine = "";
            int linesCount = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lastLine = line;
                linesCount++;
            }
            
            int runIndex = 1;
            if (linesCount != 1 && int.TryParse(lastLine.Split(',')[0].Split('R')[1], out int i))
                runIndex = i+1;

            if (linesCount != runIndex)
            {
                logSystem.AbortLog("LOGGING ERROR: " + filename + " rows are mucked up.");
                return "";
            }

            return runPrefix + runIndex.ToString(countFormat);
        }
    }

    public void StartLogging(LogSystem logSystem)
    {

        string path = logSystem.directory + "/" + filename;

        if (!File.Exists(path))
        {
            using (StreamWriter writer = WriteStream(path))
            {
                if (writer is null)
                    return;
                logging = true;
                writer.WriteLine(header);
            }
        }

        using (StreamReader reader = ReadStream(path, logSystem))
        {
            if (reader is null)
                return;
            canLog = true;
            
            startTime = System.DateTime.Now;

        }

    }

    public void StopLogging(LogSystem logSystem)
    {
        string path = logSystem.directory + "/" + filename;
        if (canLog)
        {
            using (StreamWriter writer = WriteStream(path))
            {
                if (writer is null)
                    return;
            
                closeTime = System.DateTime.Now;

                pathProximityPercentage = logSystem.pathProximity.proximityPercentage;

                line =
                    logSystem.runId + delimiter +
                    logSystem.userId + delimiter +
                    logSystem.gestureSetId + delimiter +
                    logSystem.roundId + delimiter +
                    startTime + delimiter +
                    closeTime + delimiter +
                    finishReached + delimiter +
                    totalDuration + delimiter +
                    totalTravelDuration / 1000 + delimiter +
                    backwardsTravelDuration / 1000 + delimiter +
                    rotateYDuration / 1000 + delimiter +
                    rotateXDuration / 1000 + delimiter +
                    rotateWhileTravelDuration / 1000 + delimiter +
                    idleControllerDuration / 1000 + delimiter +
                    timesStopped + delimiter +
                    distanceTraveled + delimiter +
                    pathProximityPercentage + delimiter +
                    totalCollisions + delimiter +
                    wallCollisions + delimiter +
                    fenceCollisions + delimiter +
                    propCollisions + delimiter +
                    roadblockCollisions + delimiter +
                    invisibleCollisions + delimiter +
                    pickups + delimiter +
                    totalPickups + delimiter +
                    pickupSearch;

                writer.WriteLine(line);
                writer.Close();
                logging = false;
            }
        }        
    }

    private StreamWriter WriteStream(string path)
    {
        if (path is null)
        {
            Debug.Log("You did not supply a file path.");
            return null;
        }

        try
        {
            var fs = new FileStream(path, FileMode.Append);
            return new StreamWriter(fs);
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
        {
            Debug.Log("There is a sharing violation.");
        }
        catch (IOException e)
        {
            Debug.Log($"An exception occurred:\nError code: " +
                              $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");

        }
        return null;
    }

    private StreamReader ReadStream(string path, LogSystem logSystem)
    {
        if (path is null)
        {
            Debug.Log("You did not supply a file path.");
            return null;
        }

        try
        {
            var fs = new FileStream(path, FileMode.Open);
            return new StreamReader(fs);
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
        {
            Debug.Log("There is a sharing violation.");
            logSystem.AbortLog("SETUP ERROR: " + filename + " is open. Close file and rerun.");
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
        {
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
