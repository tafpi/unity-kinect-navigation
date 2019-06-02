// https://forum.unity.com/threads/recording-a-players-movement-during-a-game-in-xyz-coordinates-then-outputting-to-a-file.257255/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class PathLogger : MonoBehaviour
{

    string FILE_NAME = "positionFile.txt";
    int time = 0;
    int interval = 150;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (time > interval)
        {
            float x = transform.position.x;
            float z = transform.position.z;
            float fx = transform.forward.x;
            float fz = transform.forward.z;
            StreamWriter sw = File.AppendText(FILE_NAME);
            sw.WriteLine("my  position is " + x + " my z position is " + z);
            sw.WriteLine("I'm facing " + transform.forward);
            sw.Close();
            Debug.Log("write to file");
            time = 0;
        }
        else
            time++;
    }
}