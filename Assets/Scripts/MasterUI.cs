using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class MasterUI : MonoBehaviour
{
    public MasterController MCScript;
    public GameObject myTrack;
    public GameObject myBall;

    public void SetTrackAsPrefab()
    {
        MCScript.myPrefab = myTrack;
        MCScript.nudge = 0;
    }

    public void SetBallAsPrefab()
    {
        MCScript.myPrefab = myBall;
        MCScript.nudge = 0.05f;
    }
}
