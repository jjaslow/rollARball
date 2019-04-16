//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class MasterController : MonoBehaviour
    {
        public Camera FirstPersonCamera;
        CheckFeaturePoints CFP;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;
        Anchor anchor;

        public GameObject myBall;
        public GameObject[] myTrack = new GameObject[3];

        public int trackNumber = 0;
        bool gameStarted = false;
        Vector3 trackStart;
        Quaternion trackRotation;
        float nudgeBallUp = .1f;
        public int numbOfBalls;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        private void Start()
        {
            CFP = GetComponent<CheckFeaturePoints>();
            numbOfBalls = 0;
        }


        public void Update()
        {
            _UpdateApplicationLifecycle();            

            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && !gameStarted)
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    DetectedPlane myPlane = hit.Trackable as DetectedPlane;
                    Debug.Log("jj_plane rotation_B: " + myPlane.CenterPose.rotation.eulerAngles);

                    // Instantiate track at the hit pose.
                    trackStart = hit.Pose.position;
                    trackStart.y += .025f; //raise track a bit off the floor
                    trackRotation = hit.Pose.rotation;
                    Debug.Log("jj_pose_a: "+ trackStart + ", " + trackRotation.eulerAngles);
                    GameObject nextTrack = Instantiate(myTrack[1], trackStart, Quaternion.Euler(0, FirstPersonCamera.transform.rotation.y, 0));  //trackStart, trackRotation   //hit.Pose.rotation 
                    nextTrack.name = "Track#" + trackNumber;
                    trackNumber++;

                    // Create an anchor FOR TRACK to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    //Pose anchorPose = new Pose(trackStart, trackRotation);
                    anchor = hit.Trackable.CreateAnchor(hit.Pose);   //anchorpose

                    // Make track a child of the anchor.
                    nextTrack.transform.parent = anchor.transform;

                    // Make sure track is pointing forward
                    /*float trackRotationFix = anchor.transform.rotation.eulerAngles.y *-1;
                    Vector3 trackRotationTurn = new Vector3(0, trackRotationFix, 0);
                    Debug.Log(nextTrack.transform.rotation.eulerAngles);
                    nextTrack.transform.Rotate(trackRotationTurn, Space.World);
                    Debug.Log(nextTrack.transform.rotation.eulerAngles);*/

                    // Instantiate ball at the hit pose.
                    Vector3 nudgedPosition = new Vector3(trackStart.x, trackStart.y + nudgeBallUp, trackStart.z);
                    GameObject newBall = Instantiate(myBall, nudgedPosition, trackRotation);  //hit.Pose.rotation   (new idea, rotation = camera.forward?)
                    numbOfBalls += 1;

                    gameStarted = true;
                }
            }
        }
        
        public void ReloadBall()
        {
            if (numbOfBalls > 0) return;

            int currTrackNumber = trackNumber - 1;
            string currTrackName = "Track#" + currTrackNumber.ToString();
            GameObject currTrack = GameObject.Find(currTrackName);

            Vector3 nudgedPosition = new Vector3(currTrack.transform.position.x, currTrack.transform.position.y + nudgeBallUp, currTrack.transform.position.z);
            GameObject newBall = Instantiate(myBall, nudgedPosition, Quaternion.identity);  //hit.Pose.rotation   (new idea, rotation = camera.forward?)
            numbOfBalls += 1;
        }


        public void NewTrack(Collider coll)
        {
            Debug.Log("JJ_trigger: " + coll.name);
            int childTriggerToDestroy, originTrackInstantiatePoint, newTrackInstantiatePoint;

            if (coll.name == "TriggerF") //destroy back trigger on new track and instantiate at front transform
            {
                childTriggerToDestroy = 1;
                originTrackInstantiatePoint = 2;
                newTrackInstantiatePoint = 3;
            }
            else  //destroy front trigger on new track and instantiate at back transform
            {
                childTriggerToDestroy = 1;
                originTrackInstantiatePoint = 3;
                newTrackInstantiatePoint = 3;
            }


            //instantiate new track piece
            //int i = Random.Range(0, myTrack.Length);
            int i = CFP.InitializePointsCheck(originTrackInstantiatePoint);
            Debug.Log("jj_track: "+i);

            if(i==-1)//no more new track
            {
                Debug.Log("GAME OVER");
                return;
            }
            else if(i==0)//left (then straight)
            {
                BuildNewTrack(coll, i, childTriggerToDestroy, originTrackInstantiatePoint, newTrackInstantiatePoint);
            }
            else if (i == 2)//right (then straight)
            {
                BuildNewTrack(coll, i, childTriggerToDestroy, originTrackInstantiatePoint, newTrackInstantiatePoint);
            }
            else if (i == 1)//straight
            {
                BuildNewTrack(coll, i, childTriggerToDestroy, originTrackInstantiatePoint, newTrackInstantiatePoint);
            }



        }

        private void BuildNewTrack(Collider coll, int i, int childTriggerToDestroy, int originTrackInstantiatePoint, int newTrackInstantiatePoint)
        {
            GameObject nextTrack = Instantiate(myTrack[i], coll.gameObject.transform.parent.GetChild(originTrackInstantiatePoint).position, coll.gameObject.transform.parent.GetChild(originTrackInstantiatePoint).rotation);

            Vector3 newStartPosition = nextTrack.transform.GetChild(newTrackInstantiatePoint).position;
            Vector3 oldEndPosition = coll.gameObject.transform.parent.GetChild(originTrackInstantiatePoint).position;
            float moveDistance = Vector3.Distance(newStartPosition, oldEndPosition);

            Debug.Log("jj_" + nextTrack.name + ": " + newStartPosition.x + ", " + newStartPosition.y + ", " + newStartPosition.z + " // " + oldEndPosition.x + ", " + oldEndPosition.y + ", " + oldEndPosition.z);
            Debug.Log(" jj_distance: " + moveDistance);

            nextTrack.transform.Translate(Vector3.forward * moveDistance);

            Destroy(nextTrack.gameObject.transform.GetChild(childTriggerToDestroy).GetComponent<BoxCollider>());

            nextTrack.transform.parent = anchor.transform;

            nextTrack.name = "Track#" + trackNumber;
            trackNumber++;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
