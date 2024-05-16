using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using System.Globalization;


namespace Imagine.WebAR
{
    public partial class WorldTracker
    {

        [System.Serializable]
        public class Settings3DOFOrbit
        {
            public float orbitDistance = 0.4f;
            //public bool useExtraSmoothing = false; //extra smoothing by default
            [Range(1,50)] public float smoothenFactor = 10;

            public Transform centerTransform;
            
            public bool swipeToRotate = true;
            public float swipeSensitivity = 0.25f;

            public bool pinchToScale = true;
            public float minDist = 0.25f;
            public float maxDist = 2.5f;
        }

        private Quaternion orbitModeOffset;
        private bool isDragging = false;
        private Vector3 startDragPos;
        private Vector3 startRot;
        private Quaternion lastRotOffset = Quaternion.identity;
        private bool isPinching = false;
        private Vector2 touch0StartPos, touch1StartPos;
        private float origDist, startDist;

        void Awake_3DOF_Orbit()
        {
        }

        void Start_3DOF_Orbit()
        {            
            Input.multiTouchEnabled = true;

            var json = "{";
            json += "\"MODE\":\"3DOF_ORBIT\",";
            json += "\"USE_COMPASS\":" + (useCompass?"true":"false");

            json += "}";
#if !UNITY_EDITOR && UNITY_WEBGL
            SetWebGLwTrackerSettings(json);
#endif        
            orbitModeOffset = Quaternion.identity;
        }

        void Update_3DOF_Orbit()
        {
            //update pos
            trackerCamera.transform.position = Vector3.Lerp(trackerCamera.transform.position, targetPos3DOF, Time.deltaTime * s3dof_orbit.smoothenFactor);
            trackerCamera.transform.rotation = Quaternion.Slerp(trackerCamera.transform.rotation, targetRot3DOF, Time.deltaTime * s3dof_orbit.smoothenFactor);
            //Debug.Log("Update Orbit Mode: " + targetPos3DOF + " " + targetRot3DOF);

            //swipe to rotate
            Update_3DOF_Orbit_SwipeToRotate();

            //pinch
            Update_3DOF_Orbit_PinchToScale();
            
        }

        void Update_3DOF_Orbit_SwipeToRotate()
        {
            if (Input.touchCount > 1)
            {
                isDragging = false;
                //return;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startDragPos = Input.mousePosition;
                    isDragging = true;
                }

                else if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                    lastRotOffset = orbitModeOffset;
                }

                else if (isDragging)
                {
                    var curDragPos = Input.mousePosition;

                    var x = curDragPos.x - startDragPos.x;
                    var rotY = x * s3dof_orbit.swipeSensitivity * 1;

                    orbitModeOffset = lastRotOffset * Quaternion.Euler(0, rotY, 0);

                }
            }  
        }

        void Update_3DOF_Orbit_PinchToScale()
        {
            if (Input.touchCount == 2)
            {
                Debug.Log("Double touch");

                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Began ||
                    touch1.phase == TouchPhase.Began)
                {
                    touch0StartPos = touch0.position;
                    touch1StartPos = touch1.position;
                    isPinching = true;
                    startDist = s3dof_orbit.orbitDistance;

                    Debug.Log("Start Pinch");
                }
                else if (touch0.phase == TouchPhase.Ended ||
                    touch1.phase == TouchPhase.Ended ||
                    touch0.phase == TouchPhase.Canceled ||
                    touch1.phase == TouchPhase.Canceled)
                {
                    isPinching = false;
                    Debug.Log("End Pinch");
                }

                if (isPinching)
                {
                    var dStart = (touch1StartPos - touch0StartPos).magnitude;

                    var pos0 = touch0.position;
                    var pos1 = touch1.position;
                    var d = (pos1 - pos0).magnitude;

                    var dist = d / dStart;
                    s3dof_orbit.orbitDistance = Mathf.Clamp(startDist / dist, s3dof_orbit.minDist, s3dof_orbit.maxDist);

                    //Debug.Log("Pinch " + scale.ToString("0.00") + "x");
                }


            }
            else
            {
                isPinching = false;
            }
        }

        void UpdateCameraTransform_3DOF_Orbit(string data)
        {
            var vals = data.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

            trackerCamRot.w = float.Parse(vals[0], CultureInfo.InvariantCulture);
            trackerCamRot.x = float.Parse(vals[1], CultureInfo.InvariantCulture);
            trackerCamRot.y = float.Parse(vals[2], CultureInfo.InvariantCulture);
            trackerCamRot.z = float.Parse(vals[3], CultureInfo.InvariantCulture);

            trackerCamRot = orbitModeOffset * trackerCamRot;
            trackerCamPos = s3dof_orbit.centerTransform.position - (trackerCamRot * Vector3.forward) * s3dof_orbit.orbitDistance;
            targetPos3DOF = trackerCamPos;
            targetRot3DOF = trackerCamRot;
            // Debug.Log("Orbit Mode: " + targetPos3DOF + " " + targetRot3DOF.eulerAngles);
                
        }

        void Reset_3DOF_Orbit()
        {
            
        }
    }
}
