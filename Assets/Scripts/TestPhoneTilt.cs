using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class TestPhoneTilt : MonoBehaviour
{
    public MasterController MCScript;

    public bool CheckTilt()
    {
        if (-Input.acceleration.y == 0) return true;   //FOR INSTANT PREVIEW TESTING. REMOVE WHEN DONE!!!!!!!!!!!!!!!!!!!!!!!!!

        if (-Input.acceleration.y <.4)
        {
            Debug.Log("too low: "+ Input.acceleration.y);
            MCScript._ShowAndroidToastMessage("too low");
            return false;
        }
        else if (-Input.acceleration.y >.6)
        {
            Debug.Log("too high: "+ Input.acceleration.y);
            MCScript._ShowAndroidToastMessage("too high");
            return false;
        }
        else return true;

    }
}
