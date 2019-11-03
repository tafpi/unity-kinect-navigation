// https://forum.unity.com/threads/recording-a-players-movement-during-a-game-in-xyz-coordinates-then-outputting-to-a-file.257255/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class LogPath : MonoBehaviour
{
    //public GameObject player;
    public GameObject groundPlane;
    //private int multiplier = 1;

    private string directoryName = "UserTesting";
    private string fileName = "positionFile";
    private int time = 0;
    private int interval = 20;
    private float x;
    private float z;
    private string polylineCoordinates = "";
    private StreamWriter logFile;
    private int fileCount;
    private string fmt = "0000.##";

    private float groundWidth;
    private float groundHeight;
    private float groundScaleX;
    private float groundScaleZ;

    // Use this for initialization
    void Start()
    {
        // if directory doesn't exit, create it
        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        fileCount = 0;
        do
        {
            fileCount++;
        }
        while (File.Exists(directoryName + "/" + fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "")+ ".svg"));


        groundWidth = groundPlane.GetComponent<Renderer>().bounds.size.x;
        groundHeight = groundPlane.GetComponent<Renderer>().bounds.size.z;
        groundScaleX = groundPlane.transform.localScale.x;
        groundScaleZ = groundPlane.transform.localScale.z;
        Debug.Log(groundWidth+", "+groundHeight);

        string groundPath =
            "<rect x='0' y='0' width='"+groundWidth+"' height='"+groundHeight+"' fill='none' />";

        string objectPaths =
            "";
        
        logFile = File.AppendText(directoryName + "/" + fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".svg");
        logFile.WriteLine("" +
            "<!DOCTYPE svg PUBLIC ' -//W3C//DTD SVG 1.1//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd'>" +
            "<svg version = '1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' xml:space='preserve' width='"+groundWidth+"' height='"+groundHeight+"'>" +
             groundPath + objectPaths +
            "<polyline points='" +
            "");
    }

    void OnDestroy()
    {
        CloseFile();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > interval)
        {
            x = groundWidth / 2 + gameObject.transform.position.x * groundScaleX;
            z = groundHeight / 2  - gameObject.transform.position.z * groundScaleZ;
            //logFile.Write("" + x + ", " + z + " ");
            polylineCoordinates += "" + x + ", " + z + " ";
            //Debug.Log("write to file");
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
}