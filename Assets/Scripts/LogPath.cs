// https://forum.unity.com/threads/recording-a-players-movement-during-a-game-in-xyz-coordinates-then-outputting-to-a-file.257255/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class LogPath : MonoBehaviour
{
    // input
    public GameObject groundPlane;
    public string filenamePrefix = "PathsLog";
    
    
    private float x;
    private float z;
    private string polylineCoordinates = "";
    private StreamWriter logFile;
    private int fileCount;

    // step distance
    private Vector3 posPrev;
    private Vector3 pos;
    //private LogRun runLogger;

    // init vars
    public bool canLog;
    //private LogSystem logSystem;
    //private string path;
    private string filename = "PathsFile.svg";
    private string content;
    private float groundWidth;
    private float groundHeight;
    private float groundScaleX;
    private float groundScaleZ;

    // other
    private Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;
    //private PathProximity pathProximity;
    //private float proximityPercentage;

    private void Start()
    {
        
    }

    public void OnUpdate(LogSystem logSystem)
    {
        posPrev = pos;
        pos = logSystem.player.transform.position;
        //logSystem.runLogger.UpdateDistanceTraveled(Vector3.Distance(pos, posPrev));

        x = groundWidth / 2 + logSystem.player.transform.position.x * groundScaleX;
        z = groundHeight / 2 - logSystem.player.transform.position.z * groundScaleZ;
        polylineCoordinates += "" + x + ", " + z + " ";
    }
    
    public void StartLogging(LogSystem logSystem)
    {
        // Get components
        //logSystem = GetComponent<LogSystem>();
        //runLogger = GetComponent<LogRun>();
        //pathProximity = logSystem.player.GetComponent<PathProximity>();

        // Create gradient
        gradient = new Gradient();
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[3];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = 0.5f;
        colorKey[2].color = Color.green;
        colorKey[2].time = 1.0f;
        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;
        gradient.SetKeys(colorKey, alphaKey);

        // Set variables
        string path = logSystem.directory + "/" + filename;
        pos = Vector3.zero;
        groundWidth = groundPlane.GetComponent<Renderer>().bounds.size.x;
        groundHeight = groundPlane.GetComponent<Renderer>().bounds.size.z;
        groundScaleX = groundPlane.transform.localScale.x;
        groundScaleZ = groundPlane.transform.localScale.z;

        if (!File.Exists(path))
        {
            using (StreamWriter writer = WriteStream(path))
            {
                if (writer is null)
                    return;
                
                // initiate paths log file
                writer.WriteLine("<!DOCTYPE svg PUBLIC ' -//W3C//DTD SVG 1.1//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd'>");
                writer.WriteLine("<svg version = '1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' xml:space='preserve' width='" + groundWidth + "' height='" + groundHeight + "'>");
                writer.WriteLine("<rect x='0' y='0' width='" + groundWidth + "' height='" + groundHeight + "' fill='#444444' />");
                writer.WriteLine("</svg>");
            }
        }

        using (StreamReader reader = ReadStream(path, logSystem))
        {
            if (reader is null)
                return;
            canLog = true;
            content = reader.ReadToEnd().Split(new string[] { "</svg>" }, StringSplitOptions.None)[0];
        }

    }

    public void StopLogging(LogSystem logSystem)
    {
        string path = logSystem.directory + "/" + filename;
        if (canLog)
        {
            // close the file if there is one
            using (FileStream stream = new FileStream(@""+path, FileMode.Create))
            using (TextWriter writer = new StreamWriter(stream))
            {
                if (writer is null)
                    return;
                writer.Write(content);
                writer.WriteLine("<polyline id='" 
                    + logSystem.runId + "-" 
                    + logSystem.userId + "-" 
                    + logSystem.gestureSet + "-" 
                    + logSystem.round 
                    + "' points='" + polylineCoordinates + "' fill='none' stroke='#" + ColorUtility.ToHtmlStringRGB(gradient.Evaluate((float)logSystem.pathProximity.proximityPercentage / 100)) + "' stroke-width='1' />");
                writer.WriteLine("</svg>");
                writer.Close();
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
            Debug.Log($"An exception occurred:\nError code: " + $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
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
            Debug.Log($"An exception occurred:\nError code: " + $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
        }
        return null;
    }
}