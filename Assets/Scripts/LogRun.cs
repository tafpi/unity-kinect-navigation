using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LogRun : MonoBehaviour
{
    // Attach as component to player

    // input
    public bool isFirstRun;

    // computed at start
    public int runId;
    public GameObject gestureTravel;
    public GameObject gestureRotateY;
    public GameObject gestureRotateX;
    public System.DateTime start;
    public int totalPickups;

    // computed on update
    public float totalDuration;
    public float travelDuration;
    public float rotateYDuration;
    public float rotateXDuration;
    public bool endReached;
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
    public System.DateTime finish;

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
        "start, " +
        "finish, " +
        "end reached, " +
        "total duration, " +
        "travel duration, " +
        "rotate y duration, " +
        "rotate x duration, " +
        "idle controller duration, " +
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
    
    private void Start()
    {
        start = System.DateTime.Now;
    }

    private void Update()
    {
        totalDuration = Time.realtimeSinceStartup;
    }

    // methods used externally

    public void CreateFile(string directory)
    {
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
            runId = RunId(reader);
        }

    }

    public int RunId(StreamReader r)
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

    public void CloseFile()
    {
        using (StreamWriter writer = WriteStream(path))
        {
            if (writer is null)
                return;
            
            finish = System.DateTime.Now;

            line =
                runId + delimiter +
                isFirstRun + delimiter +
                gestureTravel + delimiter +
                gestureRotateY + delimiter +
                gestureRotateX + delimiter +
                start + delimiter +
                finish + delimiter +
                endReached + delimiter +
                totalDuration + delimiter +
                travelDuration + delimiter +
                rotateYDuration + delimiter +
                rotateXDuration + delimiter +
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
