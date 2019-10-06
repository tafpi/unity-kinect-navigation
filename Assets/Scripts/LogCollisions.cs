using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LogCollisions : MonoBehaviour
{
    // attach as component to player

    public bool keepTrack;
    public GameObject player;
    private string fileName = "collisionsFile";
    private StreamWriter sw;
    private string fmt = "0000.##";
    public int fileCount;

    public GameObject[] walls;

    private int colIndex;

    // Start is called before the first frame update
    void Start()
    {
        fileCount = 0;
        do
        {
            fileCount++;
        } while (File.Exists(fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".csv"));

        sw = File.AppendText(fileName + (fileCount > 0 ? "_" + fileCount.ToString(fmt) : "") + ".csv");

        colIndex = 0;

        foreach (var wall in walls)
        {
            Debug.Log("asd");
        }
    }

    void OnDestroy()
    {
        if (keepTrack && sw != null)
        {
            sw.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollisionBegin(GameObject wall)
    {
        wall.GetComponent<LimitWall>().collisionEnterTime = Time.deltaTime;
    }

    public void CollisionUpdate(GameObject wall)
    {
        wall.GetComponent<LimitWall>().collisionDuration = Time.deltaTime - wall.GetComponent<LimitWall>().collisionEnterTime;
    }

    public void CollisionEnd(GameObject wall)
    {
        wall.GetComponent<LimitWall>().ResetWall();
        //if (keepTrack && sw != null)
        //{
        //    sw.WriteLine(colIndex + ", " + wall.name + ", " + "");
        //}
        //colIndex++;
    }


    public void UpdateCollisions(GameObject hitObject)
    {
        if (walls.Contains(hitObject))
        {
            Debug.Log("hit object is a limit wall");
            // hit object is a limit wall
            hitObject.GetComponent<LimitWall>().playerIsColliding = true;
            if (!hitObject.GetComponent<LimitWall>().playerWasColliding)
            {
                // start colliding
                CollisionBegin(hitObject);
            }
            else
            {
                // keep colliding
                CollisionUpdate(hitObject);
            }
            hitObject.GetComponent<LimitWall>().playerWasColliding = true;




            //Debug.Log("hit object is a limit wall");
            //foreach (var wall in walls)
            //{
            //    if (GameObject.ReferenceEquals(hitObject, wall))
            //    {
            //        //Debug.Log("true");

            //        // hit object is a limit wall
            //        wall.GetComponent<LimitWall>().playerIsColliding = true;
            //        if (!wall.GetComponent<LimitWall>().playerWasColliding)
            //        {
            //            // start colliding
            //            CollisionBegin(wall);
            //        } else
            //        {
            //            // keep colliding
            //            CollisionUpdate(wall);
            //        }
            //        wall.GetComponent<LimitWall>().playerWasColliding = true;
            //    }
            //    //else
            //    //{
            //    //    Debug.Log("false");

            //    //    if (wall.GetComponent<LimitWall>().playerWasColliding)
            //    //    {
            //    //        // stop colliding
            //    //        CollisionEnd(wall);
            //    //        wall.GetComponent<LimitWall>().ResetWall();
            //    //    }
            //    //}
            //}


        }
        //else
        //{
        //    Debug.Log("hit object is not a limit wall");
        //}
    }

    private void FixedUpdate()
    {
        foreach (var wall in walls)
        {
            wall.GetComponent<LimitWall>().playerIsColliding = false;
            if (wall.GetComponent<LimitWall>().playerWasColliding)
            {
                CollisionEnd(wall);
            }
        }
    }

}
