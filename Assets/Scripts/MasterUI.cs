using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;
using UnityEngine.UI;

public class MasterUI : MonoBehaviour
{
    public MasterController MCScript;
    public GameObject myTrack;
    public GameObject myBall;
    Text testingText;

    private void Start()
    {
        testingText = GameObject.Find("TestingText").GetComponent<Text>();
        testingText.text = null;
    }

    private void FixedUpdate()
    {
        testingText.text = Mathf.Floor(Input.acceleration.x * 100) / 100 + " \n " + Mathf.Floor(Input.acceleration.y * 100) / 100 + " \n " + Mathf.Floor(Input.acceleration.z * 100) / 100;
    }

    public void ReloadBall() //now reload
    {
        //MCScript.myPrefab = myTrack;
        //MCScript.nudge = 0;
        MCScript.ReloadBall();
    }

    public void SetBallAsPrefab()
    {
        //MCScript.myPrefab = myBall;
        //MCScript.nudge = 0.05f;
    }
}
