using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class testing : MonoBehaviour
{
    //MasterController MCScript;

    private Rigidbody RB;
    public float smooth;

    public static int ballsCreated = 0;
    public static GameObject lastTrackTouched;

    void Start()
    {
        ballsCreated++;

        smooth = 0.75f;
        //MCScript = GameObject.Find("Example Controller").GetComponent<MasterController>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1)  //based on gravity
        {
            Destroy(gameObject);
            //MCScript.numbOfBalls -= 1;
        }
    }

    private void FixedUpdate()
    {
        RB.AddForce(Vector3.forward);

        RaycastHit lastTrackHit;
        Debug.DrawRay(transform.position, Vector3.down * 2, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out lastTrackHit))
        {
            lastTrackTouched = lastTrackHit.transform.gameObject;
            Debug.Log(lastTrackTouched.name);
            /*if (lastTrackTouched.tag == "loop")
            {
                RB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
            else
            {
                RB.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }*/
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "boost")
        {
            Debug.Log("jj_boost: " + Vector3.Dot(RB.velocity.normalized, other.transform.up));
            if (Vector3.Dot(RB.velocity.normalized, other.transform.up) > .5f)
            {
                RB.velocity = Vector3.zero;
                RB.angularVelocity = Vector3.zero;
                RB.AddForce(other.transform.up * 1000);
            }
        }
        else
        {
            Destroy(other);
            //MCScript.NewTrack(other);
        }

    }
}