using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class BallMotion : MonoBehaviour
{
    MasterController MCScript;

    private Rigidbody RB;
    public float smooth;
    public int boostSpeed;
    static Vector3 initialAcceleration;
   
    public static int totalBallsCreated = 0;
    public static GameObject lastTrackTouched;

    void Start()
    {
        totalBallsCreated++;
        if (totalBallsCreated == 1)
        {
            initialAcceleration = Input.acceleration;
        }
        smooth = 0.75f;
        MCScript = GameObject.Find("Example Controller").GetComponent<MasterController>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -2)  //based on gravity
        {
            Destroy(gameObject);
            MCScript.numBallsOnTrack -= 1;
            return;
        }

        RaycastHit lastTrackHit;
        Debug.DrawRay(transform.position, Vector3.down * 2, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out lastTrackHit))
        {
            lastTrackTouched = lastTrackHit.transform.gameObject;
            if (lastTrackTouched.tag == "childTrack") lastTrackTouched = lastTrackTouched.transform.parent.gameObject;
        }
    }

    private void FixedUpdate()
    {
        Vector3 motion = Input.acceleration;
        motion.y -= initialAcceleration.y;
        motion.y = Mathf.Clamp(motion.y, -.5f, .5f);
        //motion.x -= initialAcceleration.x;
        //motion.x = Mathf.Clamp(motion.x, -.5f, .5f);

        Debug.Log("jj_motion: " + motion);
        RB.AddForce(Mathf.Floor(motion.x*100)/100 * smooth, 0, Mathf.Floor(motion.y*100)/ 100 * smooth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "boost")
        {
            //RB.collisionDetectionMode = CollisionDetectionMode.Discrete;
            Debug.Log("jj_boost: " + Vector3.Dot(RB.velocity.normalized, other.transform.up));
            if(Vector3.Dot(RB.velocity.normalized, other.transform.up) > .5f)
            {
                RB.velocity = Vector3.zero;
                RB.angularVelocity = Vector3.zero;
                //RB.collisionDetectionMode = CollisionDetectionMode.Continuous;
                RB.AddForce(other.transform.up * boostSpeed);
            }
        }
        else
        {
            Destroy(other);
            MCScript.NewTrack(other);
        }
        
    }
}
