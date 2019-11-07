// https://forum.unity.com/threads/recording-a-players-movement-during-a-game-in-xyz-coordinates-then-outputting-to-a-file.257255/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class LogPath : MonoBehaviour
{
    // input
    public GameObject groundPlane;
    public string filenamePrefix = "PathFile";
    
    private int time = 0;
    private int interval = 20;
    private float x;
    private float z;
    private string polylineCoordinates = "";
    private StreamWriter logFile;
    private int fileCount;

    // init vars
    private float groundWidth;
    private float groundHeight;
    private float groundScaleX;
    private float groundScaleZ;

    // Use this for initialization
    void Start()
    {
        groundWidth = groundPlane.GetComponent<Renderer>().bounds.size.x;
        groundHeight = groundPlane.GetComponent<Renderer>().bounds.size.z;
        groundScaleX = groundPlane.transform.localScale.x;
        groundScaleZ = groundPlane.transform.localScale.z;
    }
    
    public void OnUpdate(GameObject player)
    {
        if (time > interval)
        {
            x = groundWidth / 2 + player.transform.position.x * groundScaleX;
            z = groundHeight / 2  - player.transform.position.z * groundScaleZ;
            polylineCoordinates += "" + x + ", " + z + " ";
            time = 0;
        }
        else
        {
            time++;
        }
    }

    public void CloseFile()
    {
        // close the file if there is one
        if (logFile != null)
        {
            if (logFile.BaseStream != null)
            {
                logFile.WriteLine(polylineCoordinates + "' fill='none' stroke='#000000' stroke-width='3' /></svg>");
                logFile.Close();
            }
        }
    }

    public void CreateFile(string path, string suffixFormat)
    {
        string filename;
        fileCount = 0;
        do
        {
            fileCount++;
            filename = path + "/" + filenamePrefix + (fileCount > 0 ? "_" + fileCount.ToString(suffixFormat) : "") + ".svg";
        } while (File.Exists(filename));

        string groundPath = "<rect x='0' y='0' width='" + groundWidth + "' height='" + groundHeight + "' fill='none' />";
        string objectPaths = "";

        logFile = File.AppendText(filename);
        logFile.WriteLine("" +
            "<!DOCTYPE svg PUBLIC ' -//W3C//DTD SVG 1.1//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd'>" +
            "<svg version = '1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' xml:space='preserve' width='" + groundWidth + "' height='" + groundHeight + "'>" +
             groundPath + objectPaths +
            "<polyline points='" +
            "");
    }
}