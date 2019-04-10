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
        float nudgeBallUp = .15f;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

  
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
                    Debug.Log("jj_plane rotation_B: " + myPlane.CenterPose.rotation.ToEuler().x * 100 + ", " + myPlane.CenterPose.rotation.ToEuler().y * 100 + ", " + myPlane.CenterPose.rotation.ToEuler().z * 100);

                    // Instantiate track at the hit pose.
                    trackStart = hit.Pose.position;
                    trackStart.y += .05f; //raise track a bit off the floor
                    trackRotation = hit.Pose.rotation;
                    Debug.Log("jj_pose_a: "+ trackStart + ", " + trackRotation.ToEuler());
                    trackRotation = Quaternion.Euler(0, -myPlane.CenterPose.rotation.ToEuler().y*2, 0)   ;
                    Debug.Log("jj_pose_b: " + trackStart + ", " + trackRotation.ToEuler());
                    GameObject nextTrack = Instantiate(myTrack[0], trackStart, trackRotation);  //hit.Pose.rotation 
                    nextTrack.name = "Track#" + trackNumber;
                    trackNumber++;

                    // Create an anchor FOR TRACK to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    Pose anchorPose = new Pose(trackStart, trackRotation);
                    anchor = hit.Trackable.CreateAnchor(anchorPose);

                    // Make track a child of the anchor.
                    nextTrack.transform.parent = anchor.transform;


                    // Instantiate ball at the hit pose.
                    Vector3 nudgedPosition = new Vector3(trackStart.x, trackStart.y + nudgeBallUp, trackStart.z);
                    GameObject newBall = Instantiate(myBall, nudgedPosition, trackRotation);  //hit.Pose.rotation   (new idea, rotation = camera.forward?)

                    gameStarted = true;
                }
            }
        }
        
        public void ReloadBall()
        {
            int currTrackNumber = trackNumber - 1;
            string currTrackName = "Track#" + currTrackNumber.ToString();
            GameObject currTrack = GameObject.Find(currTrackName);

            Vector3 nudgedPosition = new Vector3(currTrack.transform.position.x, currTrack.transform.position.y + nudgeBallUp, currTrack.transform.position.z);
            GameObject newBall = Instantiate(myBall, nudgedPosition, Quaternion.identity);  //hit.Pose.rotation   (new idea, rotation = camera.forward?)
        }


        public void NewTrack(Collider coll)
        {
            //Debug.Log("trigger: " + coll.name);
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
            int i = Random.Range(0, myTrack.Length);
            GameObject nextTrack = Instantiate(myTrack[i], coll.gameObject.transform.parent.GetChild(originTrackInstantiatePoint).position, coll.gameObject.transform.parent.GetChild(originTrackInstantiatePoint).rotation);

            /*if(nextTrack.GetComponent<MeshFilter>().mesh.bounds.center.z==0)
            {
                nextTrack.transform.Translate(Vector3.forward * nextTrack.GetComponent<MeshFilter>().mesh.bounds.extents.z/2);
            }*/
            Vector3 newStartPosition = nextTrack.transform.GetChild(newTrackInstantiatePoint).position;
            Vector3 oldEndPosition = coll.gameObject.transform.parent.GetChild(originTrackInstantiatePoint).position;
            float moveDistance = Vector3.Distance(newStartPosition, oldEndPosition);
            Debug.Log("jj_" + nextTrack.name + ": " + newStartPosition.x *100 + ", " + newStartPosition.y * 100 + ", " + newStartPosition.z * 100 + " // " + oldEndPosition.x*100 + ", " + oldEndPosition.y * 100 + ", " + oldEndPosition.z * 100);
            Debug.Log(" jj_distance: " + moveDistance);
            //Vector3 movePosition = oldEndPosition - newStartPosition;
            //nextTrack.transform.Translate(Vector3.forward * (Mathf.Abs(movePosition.x) + Mathf.Abs(movePosition.y) + Mathf.Abs(movePosition.z)));
            //nextTrack.transform.Translate(Vector3.forward * Mathf.Abs(movePosition.x + movePosition.y + movePosition.z));
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
