using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;
using UnityEngine.UI;

public class MasterUI : MonoBehaviour
{
    public MasterController MCScript;
    public CheckFeaturePoints CFP;
    Text testingText;

    private void Start()
    {
        testingText = GameObject.Find("TestingText").GetComponent<Text>();
        testingText.text = null;
    }

    private void FixedUpdate()
    {
        testingText.text = Mathf.Floor(Input.acceleration.x * 100) / 100 + "\n" + Mathf.Floor(Input.acceleration.y * 100) / 100 + "\n" + Mathf.Floor(Input.acceleration.z * 100) / 100 + "\n" + CFP.numberOfPoints;
    }

    public void ReloadBall() //now reload
    {
        MCScript.ReloadBall();
    }

}
