using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Imagine.WebAR
{
    [System.Serializable]
    public class Settings6DOF
    {
        //public bool repositionOnTouch = false;

        public enum DepthMode {SCALE_AS_DEPTH, Z_AS_DEPTH_EXPERIMENTAL};
        public DepthMode depthMode = DepthMode.SCALE_AS_DEPTH;

       //public Vector3 camOffset;

       [Range(300, 600)] public int maxPixels = 450;
       [Range(50, 200)] public int maxPoints = 120;
    }

    public partial class WorldTracker
    {
        [DllImport("__Internal")] private static extern void WebGLPlaceOrigin(string camPosStr);
        [DllImport("__Internal")] private static extern void WebGLSyncSSPos(string vStr);


        private float startZ;


        void Awake_6DOF()
        {

        }

        void Start_6DOF()
        {
            var camPos = new Vector3(origPos.x, 0, origPos.z);
            startZ = (mainObject.transform.position - camPos).magnitude;

            var dmode = "";
            if(s6dof.depthMode == Settings6DOF.DepthMode.SCALE_AS_DEPTH)
                dmode = "SCALE";
            else if(s6dof.depthMode == Settings6DOF.DepthMode.Z_AS_DEPTH_EXPERIMENTAL)
                dmode = "Z";

            var json = "{";
            json += "\"MODE\":\"3DOF\","; //<--3DOF for placement mode only
            json += "\"DEPTHMODE\":\"" + dmode + "\",";
            json += "\"START_Z\":" + startZ.ToStringInvariantCulture() + ",";
            json += "\"CAM_START_HEIGHT\":" + cameraStartHeight.ToStringInvariantCulture() + ",";
            json += "\"ARM_LENGTH\":" + s3dof.armLength.ToStringInvariantCulture() + ",";
            json += "\"MAX_PIXELS\":" + s6dof.maxPixels + ",";
            json += "\"MAX_POINTS\":" + s6dof.maxPoints + ",";

            json += "\"USE_COMPASS\":" + (useCompass?"true":"false");

            json += "}";
#if !UNITY_EDITOR && UNITY_WEBGL
            SetWebGLwTrackerSettings(json);
#endif

        }

        public void Update_6DOF()
        {
           if (usePlacementIndicator && !placementIndicatorSettings.placed)
            {
                Update_3DOF();
            }
        }

        void UpdateCameraTransform_6DOF(string data)
        {
            //Debug.Log(data);

            if (usePlacementIndicator && !placementIndicatorSettings.placed)
            {
                UpdateCameraTransform_3DOF(data);
            }
            else
            {
                var vals = data.Split(new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);

                trackerCamRot.w = float.Parse(vals[0], CultureInfo.InvariantCulture);
                trackerCamRot.x = float.Parse(vals[1], CultureInfo.InvariantCulture);
                trackerCamRot.y = float.Parse(vals[2], CultureInfo.InvariantCulture);
                trackerCamRot.z = float.Parse(vals[3], CultureInfo.InvariantCulture);

                trackerCamPos.x = float.Parse(vals[4], CultureInfo.InvariantCulture);
                trackerCamPos.y = float.Parse(vals[5], CultureInfo.InvariantCulture);
                trackerCamPos.z = float.Parse(vals[6], CultureInfo.InvariantCulture);

                trackerCamera.transform.position = trackerCamPos;
                trackerCamera.transform.rotation = trackerCamRot;

                mainObject.transform.localScale = float.Parse(vals[7], CultureInfo.InvariantCulture) * Vector3.one;
            }

        }

        void Place_6DOF(){

            StartCoroutine("SyncScreenSpaceRoutine");

            mainObject.transform.position = Vector3.zero;
            var startCamPos = new Vector3(origPos.x, 0, origPos.z);
            trackerCamera.transform.position = startCamPos;

#if UNITY_WEBGL && !UNITY_EDITOR
            var json = "{";
            json += "\"MODE\":\"6DOF\"" + ",";
            json += "\"START_Z\":" + startZ.ToStringInvariantCulture();
            json += "}";
            SetWebGLwTrackerSettings(json);
            var camPosStr = startCamPos.x + "," + startCamPos.y + "," + startCamPos.z;
            WebGLPlaceOrigin(camPosStr);     
#endif
        }

        void Reset_6DOF()
        {
            StopCoroutine("SyncScreenSpaceRoutine");
#if UNITY_WEBGL && !UNITY_EDITOR
            SetWebGLwTrackerSettings("{\"MODE\":\"3DOF\"}");
#endif
        }

        IEnumerator SyncScreenSpaceRoutine()
        {
            var WaitForEndOfFrameRoutine = new WaitForEndOfFrame();

            while (true)
            {
                SyncScreenSpacePosition();
                yield return WaitForEndOfFrameRoutine;
                // yield return new WaitForEndOfFrame();
                //yield return new WaitForSeconds(0.2f);
            }
        }

        private void SyncScreenSpacePosition()
        {
            var ssPos = trackerCamera.cam.WorldToScreenPoint(mainObject.transform.position);
            ssPos.x /= Screen.width;
            ssPos.y /= Screen.height;
            // Debug.Log("ssPos = " + ssPos);
#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLSyncSSPos(ssPos.x.ToStringInvariantCulture() + "," + ssPos.y.ToStringInvariantCulture());
#endif

        }
    }
}
