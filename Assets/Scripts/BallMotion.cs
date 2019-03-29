using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.HelloAR;

public class BallMotion : MonoBehaviour
{

    public float smooth;
    private Vector3 currentAcceleration, initialAcceleration;
    private float initialXAcceleration, initialYAcceleration;
    RaycastHit hit;
    //Text testingText;
    MasterController MCScript;
    private Rigidbody RB;

    void Start()
    {
        initialAcceleration = Input.acceleration;
        currentAcceleration = Vector3.zero;
        initialXAcceleration = Input.acceleration.x;
        initialYAcceleration = Input.acceleration.y;
        smooth = 0.5f;
        //testingText = GameObject.Find("TestingText").GetComponent<Text>();
        //testingText.text = null;
        MCScript = GameObject.Find("Example Controller").GetComponent<MasterController>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)  //based on gravity
        {
            Destroy(gameObject);
            return;
        }

        /*Debug.DrawRay(transform.position, Vector3.down * 10, Color.red);
        if(!Physics.Raycast(transform.position, Vector3.down, out hit))  //no gravity, based on looking down to see track
        {
            Destroy(gameObject);
            return;
        }*/


        //currentAcceleration = Vector3.Lerp(currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);
        //transform.Translate(-currentAcceleration.y * Time.deltaTime, 0, currentAcceleration.x * Time.deltaTime);

        //Vector3 initialPosition = transform.position;
        //Vector3 tilt = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);
        //testingText.text = tilt.x + " x " + tilt.y + " x " + tilt.z;
        //Debug.DrawRay(transform.position + Vector3.up, tilt, Color.blue);
        //transform.Translate((Input.acceleration.x - initialXAcceleration) * Time.deltaTime, 0, (Input.acceleration.y - initialYAcceleration) * Time.deltaTime);
        //transform.Translate(tilt * Time.deltaTime);

        //transform.Translate(new Vector3(.0005f, 0, .0005f));
        //Vector3 distance = transform.position - initialPosition;

        //transform.Rotate(new Vector3(distance.x * 10000, 0, distance.z * 10000), Space.World);


    }

    private void FixedUpdate()
    {
        Vector3 motion = Input.acceleration;
        RB.AddForce(Mathf.Floor(motion.x*100)/100 * smooth, 0, Mathf.Floor(motion.y*100)/ 100 * smooth);
        //testingText.text = Mathf.Floor(Input.acceleration.x * 100) / 100 + " x " + Mathf.Floor(Input.acceleration.y * 100) / 100 + " x " + Mathf.Floor(Input.acceleration.z * 100) / 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other);
        //testingText.text = other.name;
        MCScript.NewTrack(other);
}
}
