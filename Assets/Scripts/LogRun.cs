﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LogRun : MonoBehaviour
{
    // Attach as component to finish object

    // input

    // computed at start
    //public int runId;
    //public GameObject gestureTravel;
    //public GameObject gestureRotateY;
    //public GameObject gestureRotateX;
    public System.DateTime startTime;
    public int totalPickups;

    // computed on update
    public float totalDuration;
    public float totalTravelDuration;
    public float backwardsTravelDuration;
    public float rotateYDuration;
    public float rotateXDuration;
    public float rotateWhileTravelDuration;
    public bool finishReached;
    public float idleControllerDuration;
    public int timesStopped;
    public float distanceTraveled;
    public int pathProximityPercentage;
    public int totalCollisions;
    public int wallCollisions;
    public int fenceCollisions;
    public int propCollisions;
    public int pickups;
    public float pickupSearch;

    // computed on end
    public System.DateTime closeTime;

    // file management
    public string filename = "RunsLog.csv";
    public string runPrefix = "R";
    private string delimiter = ", ";
    private int fileCount;
    private string fileContent;
    private StreamWriter logFile;
    //private string path;
    private string line;
    private string header =
        "run id, " +
        "user id, " +
        "gesture set, " +
        "round, " +
        //"gesture travel, " +
        //"gesture rotate y, " +
        //"gesture rotate x, " +
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
        "pickups, " +
        "total pickups, " +
        "pickup search";

    // logging
    public bool canLog;
    public bool logging;
    private int interval = 0;
    [Range(10, 30)] public int intervalDelay = 20;

    // other
    //private LogSystem logSystem;
    //private PickupPlayer pickupPlayer;
    //private PathProximity pathProximity;


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


    // methods used externally
    //public void UpdatePathProximity(LogSystem logSystem)
    //{
    //    pathProximityPercentage = logSystem.pathProximity.proximityPercentage;
    //}

    public void UpdateDistanceTraveled(float step)
    {
        distanceTraveled += step;        
    }

    public void StartLogging(LogSystem logSystem)
    {
        //logSystem = GetComponent<LogSystem>();
        //logSystem.playerManager.logRun = this;
        //pathProximity = logSystem.pathProximity;

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

            // logs on start
            //runId = RunId();

            startTime = System.DateTime.Now;

            //pickupPlayer = logSystem.player.GetComponentInChildren<PickupPlayer>();
            if (logSystem.pickupGroups.Length > 0)
                totalPickups = logSystem.pickupGroups.Length;

        }

    }

    public void StopLogging(LogSystem logSystem)
    {
        string path = logSystem.directory + "/" + filename;
        if (canLog)
        {
            using (StreamWriter writer = WriteStream(path))
            {
                Debug.Log(logSystem);
                if (writer is null)
                    return;
            
                closeTime = System.DateTime.Now;

                //PlayerMove playerMove = logSystem.playerMove;
                //if (playerMove)
                //    gestureTravel = playerMove.travelGesture;
                //PlayerLookLeftRight playerLookY = logSystem.GetComponent<PlayerLookLeftRight>();
                //if (playerLookY)
                //    gestureRotateY = playerLookY.rotateGesture;
                //PlayerLookUpDown playerLookX = logSystem.GetComponentInChildren<PlayerLookUpDown>();
                //if (playerLookX)
                //    gestureRotateX = playerLookX.rotateGesture;

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

    private StreamReader ReadStream(string path, LogSystem logSystem)
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
            logSystem.AbortLog("SETUP ERROR: " + filename + " is open. Close file and rerun.");
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
