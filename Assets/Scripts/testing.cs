using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    public float smooth = 0.25f;
    //public float newRotation;
    //public float sensitivity = 6;
    private Vector3 currentAcceleration, initialAcceleration;
    //public GameObject bend, straight;
    public Transform otherGO;

    void Start()
    {
        //initialAcceleration = Input.acceleration;
        //currentAcceleration = Vector3.zero;
        //Input.gyro.enabled = true;
        /*Bounds bendBounds = bend.GetComponent<MeshRenderer>().bounds;
        Bounds straightBounds = straight.GetComponent<MeshRenderer>().bounds;

        Debug.Log("bend center: " + bendBounds.center.x * 100 + ", " + bendBounds.center.y * 100 + ", " + bendBounds.center.z * 100);
        Debug.Log("bend size: " + bendBounds.size.x * 100 + ", " + bendBounds.size.y * 100 + ", " + bendBounds.size.z * 100);
        Debug.Log("bend extents: " + bendBounds.extents.x * 100 + ", " + bendBounds.extents.y * 100 + ", " + bendBounds.extents.z * 100);

        Debug.Log("straight center: " + straightBounds.center.x * 100 + ", " + straightBounds.center.y * 100 + ", " + straightBounds.center.z * 100);
        Debug.Log("straight size: " + straightBounds.size.x * 100 + ", " + straightBounds.size.y * 100 + ", " + straightBounds.size.z * 100);
        Debug.Log("straight extents: " + straightBounds.extents.x * 100 + ", " + straightBounds.extents.y * 100 + ", " + straightBounds.extents.z * 100);
        */

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetDir = new Vector3(otherGO.position.x, 0, otherGO.position.z)  - new Vector3(transform.position.x, 0, transform.position.z); //other being feature point
        float angle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);
        float height = otherGO.position.y - transform.position.y;
        float distance = Vector3.Distance(otherGO.position, transform.position);
        Debug.Log("angle: " + angle + ", height: " + height + ", distance: " + distance);


        //currentAcceleration = Vector3.Lerp(currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);

        //transform.Translate(-currentAcceleration.y * Time.deltaTime, 0, currentAcceleration.x * Time.deltaTime); OLD ONE
        //transform.Translate(Input.acceleration.x * Time.deltaTime, 0, Input.acceleration.y * Time.deltaTime);
        //transform.Translate(currentAcceleration.x * Time.deltaTime, 0, currentAcceleration.y * Time.deltaTime);
        //transform.Translate(Input.acceleration.x * smooth, 0, Input.acceleration.y * smooth);
        //Debug.Log("input: " + Input.acceleration.x + " x " + Input.acceleration.y + " x " + Input.acceleration.z);
        //Debug.Log("curr: "+ currentAcceleration.x + " x " + currentAcceleration.y + " x " + currentAcceleration.z);

        //newRotation = Mathf.Clamp(currentAcceleration.x * sensitivity, -1, 1);
        //transform.Rotate(0, 0, -newRotation);


        //Vector3 initialPosition = transform.position;
        //Vector3 tilt = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);
        //Debug.Log(Input.gyro.enabled);
    }
}
