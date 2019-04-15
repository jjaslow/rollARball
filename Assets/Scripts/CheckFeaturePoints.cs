using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.HelloAR;

public class CheckFeaturePoints : MonoBehaviour
{
    public int numberOfPoints;
    public MasterController MCScript;

    //heights of points for angles in degree ranges from 0-60-120-180
    public bool PointsAngleA;
    public bool PointsAngleB;
    public bool PointsAngleC;


    // Update is called once per frame
    public int InitializePointsCheck()
    {
        PointsAngleA = true; PointsAngleB = true; PointsAngleC = true; //can go this direction
        numberOfPoints = Frame.PointCloud.PointCount;
        //Debug.Log("jj_Num of Points: " + numberOfPoints);
        if (numberOfPoints == 0) return -1;
        for (int i=0; i<numberOfPoints; i++)
        {
            PointCloudPoint myPoint = Frame.PointCloud.GetPointAsStruct(i);
            Vector3 Position = myPoint.Position;
            CheckPoint(Position, i);
        }

        List<int> PointList = new List<int>();
        if (PointsAngleA == true) PointList.Add(0);
        if (PointsAngleB == true) PointList.Add(1);
        if (PointsAngleC == true) PointList.Add(2);

        if(PointList.Count == 0) return -1;
        else
        {
            int j = PointList[Random.Range(0, PointList.Count)];
            Debug.Log("JJ_pointList: " + j);
            PointList.Clear();
            return j;
        }
        
    }


    void CheckPoint(Vector3 visiblePoints, int i)
    {
        if (MCScript.trackNumber == 0) return;  //can remove once this is called upon track trgger (bc # of tracks wont be 0)

        int currTrackNumber = MCScript.trackNumber - 1;
        string currTrackName = "Track#" + currTrackNumber.ToString();
        GameObject currTrack = GameObject.Find(currTrackName);

        Vector3 targetDir = new Vector3(visiblePoints.x, 0, visiblePoints.z) - new Vector3(currTrack.transform.position.x, 0, currTrack.transform.position.z); //other being feature point
        float angle = Vector3.SignedAngle(currTrack.transform.forward, targetDir, Vector3.up);
        float height = visiblePoints.y - currTrack.transform.position.y;
        float distance = Vector3.Distance(visiblePoints, currTrack.transform.position);
        Debug.Log("jj_Point " + i + ": " + "angle: " + angle + ", height: " + height + ", distance: " + distance);

        if (distance > .5f || height < 0.05f) return;   //if point to far away or shorter than 2" up then that point is not in our way
        else if(angle >= 0 || angle <= 60)
        {
            PointsAngleA = false;
            return;
        }
        else if (angle > 60 || angle <= 120)
        {
            PointsAngleB = false;
            return;
        }
        else if (angle > 120 || angle <= 180)
        {
            PointsAngleC = false;
            return;
        }
    }

}
