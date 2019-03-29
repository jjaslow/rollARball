using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    public float smooth = 0.25f;
    //public float newRotation;
    //public float sensitivity = 6;
    private Vector3 currentAcceleration, initialAcceleration;

    void Start()
    {
        initialAcceleration = Input.acceleration;
        currentAcceleration = Vector3.zero;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        //currentAcceleration = Vector3.Lerp(currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);

        //transform.Translate(-currentAcceleration.y * Time.deltaTime, 0, currentAcceleration.x * Time.deltaTime); OLD ONE
        //transform.Translate(Input.acceleration.x * Time.deltaTime, 0, Input.acceleration.y * Time.deltaTime);
        //transform.Translate(currentAcceleration.x * Time.deltaTime, 0, currentAcceleration.y * Time.deltaTime);
        //transform.Translate(Input.acceleration.x * smooth, 0, Input.acceleration.y * smooth);
        //Debug.Log("input: " + Input.acceleration.x + " x " + Input.acceleration.y + " x " + Input.acceleration.z);
        //Debug.Log("curr: "+ currentAcceleration.x + " x " + currentAcceleration.y + " x " + currentAcceleration.z);

        //newRotation = Mathf.Clamp(currentAcceleration.x * sensitivity, -1, 1);
        //transform.Rotate(0, 0, -newRotation);


        Vector3 initialPosition = transform.position;
        Vector3 tilt = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);
        Debug.Log(Input.gyro.enabled);
    }
}




//