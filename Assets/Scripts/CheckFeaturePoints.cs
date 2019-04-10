using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.HelloAR;

public class CheckFeaturePoints : MonoBehaviour
{
    public int numberOfPoints;
    public MasterController MCScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        numberOfPoints = Frame.PointCloud.PointCount;
        Debug.Log("jj_Num of Points: " + numberOfPoints);
        if (numberOfPoints == 0) return;
        for (int i=0; i<numberOfPoints; i++)
        {
            PointCloudPoint myPoint = Frame.PointCloud.GetPointAsStruct(i);
            Vector3 Position = myPoint.Position;
            CheckPoint(Position, i);
        }
    }


    void CheckPoint(Vector3 visiblePoints, int i)
    {
        if (MCScript.trackNumber == 0) return;

        int currTrackNumber = MCScript.trackNumber - 1;
        string currTrackName = "Track#" + currTrackNumber.ToString();
        GameObject currTrack = GameObject.Find(currTrackName);

        Vector3 targetDir = new Vector3(visiblePoints.x, 0, visiblePoints.z) - new Vector3(currTrack.transform.position.x, 0, currTrack.transform.position.z); //other being feature point
        float angle = Vector3.SignedAngle(currTrack.transform.forward, targetDir, Vector3.up);
        float height = visiblePoints.y - currTrack.transform.position.y;
        float distance = Vector3.Distance(visiblePoints, currTrack.transform.position);
        Debug.Log("jj_Point " + i + ": " + "angle: " + angle + ", height: " + height + ", distance: " + distance);
    }

}
