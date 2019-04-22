using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class BallMotion : MonoBehaviour
{

    public float smooth;
    //private Vector3 currentAcceleration;
    static Vector3 initialAcceleration;
    //private float initialXAcceleration, initialYAcceleration;
    //RaycastHit hit;
    MasterController MCScript;
    private Rigidbody RB;
    public static int ballsCreated = 0;
    public static GameObject lastTrackTouched;

    void Start()
    {
        ballsCreated++;
        if (ballsCreated <= 1)
        {
            initialAcceleration = Input.acceleration;
        }
        //currentAcceleration = Vector3.zero;
        smooth = 0.75f;
        MCScript = GameObject.Find("Example Controller").GetComponent<MasterController>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit lastTrackHit;
        Debug.DrawRay(transform.position, Vector3.down * 2, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out lastTrackHit))  //no gravity, based on looking down to see track
        {
            lastTrackTouched = lastTrackHit.transform.gameObject;
        }

        if (transform.position.y < -2.5)  //based on gravity
        {
            Destroy(gameObject);
            MCScript.numbOfBalls -= 1;
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
        motion.y -= initialAcceleration.y;
        motion.y = Mathf.Clamp(motion.y, -.5f, .5f);
        Debug.Log("jj_motion: " + motion);
        RB.AddForce(Mathf.Floor(motion.x*100)/100 * smooth, 0, Mathf.Floor(motion.y*100)/ 100 * smooth);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other);
        MCScript.NewTrack(other);
    }
}
