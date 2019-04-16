using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.HelloAR;

public class CheckFeaturePoints : MonoBehaviour
{
    public int numberOfPoints;
    public MasterController MCScript;

    //points for angles in degree ranges from 0-60-120-180
    public bool PointsAngleA;
    public bool PointsAngleB;
    public bool PointsAngleC;


    public int InitializePointsCheck(int originTrackInstantiatePoint)
    {
        PointsAngleA = true; PointsAngleB = true; PointsAngleC = true; //can go this direction
        numberOfPoints = Frame.PointCloud.PointCount;
        Debug.Log("jj_Num of Points: " + numberOfPoints);

        if (numberOfPoints > 0) //check all points to see theie angle, height and distance
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                PointCloudPoint myPoint = Frame.PointCloud.GetPointAsStruct(i);
                Vector3 Position = myPoint.Position;
                CheckPoint(Position, i, originTrackInstantiatePoint);
            }
        }

        //now choose a random option from available directions
        List<int> PointList = new List<int>();
        if (PointsAngleA == true) PointList.Add(0);
        if (PointsAngleB == true) PointList.Add(1);
        if (PointsAngleC == true) PointList.Add(2);

        if(PointList.Count == 0) return -1; //no free direction
        else
        {
            int j = PointList[Random.Range(0, PointList.Count)];
            Debug.Log("JJ_pointList: " + j);
            PointList.Clear();  //clear list for next time around
            return j;
        }
        
    }


    void CheckPoint(Vector3 PointPosition, int i, int originTrackInstantiatePoint)
    {
        //get track we are on that needs new extension
        int currTrackNumber = MCScript.trackNumber - 1;
        string currTrackName = "Track#" + currTrackNumber.ToString();
        Debug.Log("JJ_curr Track: " + currTrackName);
        GameObject currTrack = GameObject.Find(currTrackName);

        //get angle, height and distance from end of current track
        Vector3 targetDir = new Vector3(PointPosition.x, 0, PointPosition.z) - new Vector3(currTrack.transform.GetChild(originTrackInstantiatePoint).position.x, 0, currTrack.transform.GetChild(originTrackInstantiatePoint).position.z); //other being feature point
        float angle = Vector3.SignedAngle(currTrack.transform.GetChild(originTrackInstantiatePoint).forward, targetDir, Vector3.up);
        float height = PointPosition.y - currTrack.transform.position.y;
        float distance = Vector3.Distance(PointPosition, currTrack.transform.GetChild(originTrackInstantiatePoint).position);
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
