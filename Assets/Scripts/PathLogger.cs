// https://forum.unity.com/threads/recording-a-players-movement-during-a-game-in-xyz-coordinates-then-outputting-to-a-file.257255/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class PathLogger : MonoBehaviour
{
    public GameObject player;

    string fileName = "positionFile";
    int time = 0;
    int interval = 20;
    float x;
    float z;
    string polylineCoordinates = "";
    StreamWriter sw;
    public int fileCount;
    string fmt = "0000.##";

    // Use this for initialization
    void Start()
    {        
        fileCount = 0;
        do
        {
            fileCount++;
        }
        while (File.Exists(fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "")+ ".svg"));
        
        sw = File.AppendText(fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".svg");
        sw.WriteLine("<!DOCTYPE svg PUBLIC ' -//W3C//DTD SVG 1.1//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd'><svg version = '1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' xml:space='preserve' width='1000' height='1000'><polyline points='");
    }

    void OnDestroy()
    {
        sw.WriteLine(polylineCoordinates+"' fill='white' stroke='#000000' stroke-width='3' /></svg>");
        sw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > interval)
        {
            x = -player.transform.position.x * 10 + 400;
            z = player.transform.position.z * 10 + 400;
            //sw.Write("" + x + ", " + z + " ");
            polylineCoordinates += "" + x + ", " + z + " ";
            //Debug.Log("write to file");
            time = 0;
        }
        else
        {
            time++;
        }
    }
}