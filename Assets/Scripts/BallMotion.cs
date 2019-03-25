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
    Text testingText;
    public MasterController MCScript;

    void Start()
    {
        initialAcceleration = Input.acceleration;
        currentAcceleration = Vector3.zero;
        initialXAcceleration = Input.acceleration.x;
        initialYAcceleration = Input.acceleration.y;
        smooth = 0.05f;
        testingText = GameObject.Find("TestingText").GetComponent<Text>();
        MCScript = GameObject.Find("Example Controller").GetComponent<MasterController>();
        testingText.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)  //based on gravity
        {
            Destroy(gameObject);
            return;
        }

        Debug.DrawRay(transform.position, Vector3.down * 10, Color.red);
        if(!Physics.Raycast(transform.position, Vector3.down, out hit))  //no gravity, based on looking down to see track
        {
            Destroy(gameObject);
            return;
        }


        //currentAcceleration = Vector3.Lerp(currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);
        //transform.Translate(-currentAcceleration.y * Time.deltaTime, 0, currentAcceleration.x * Time.deltaTime);

        Vector3 initialPosition = transform.position;
        transform.Translate((Input.acceleration.x - initialXAcceleration) * Time.deltaTime, 0, (Input.acceleration.y - initialYAcceleration) * Time.deltaTime);
        //transform.Translate(new Vector3(.0005f, 0, .0005f));
        Vector3 distance = transform.position - initialPosition;
        
        transform.Rotate(new Vector3(distance.x * 10000, 0, distance.z * 10000), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other);
        //testingText.text = other.name;
        MCScript.NewTrack(other.name);
}
}
