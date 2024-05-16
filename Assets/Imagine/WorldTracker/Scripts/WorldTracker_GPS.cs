using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Imagine.WebAR
{

    [System.Serializable]
    public class GeolocationSettings{
        [SerializeField] public float activationRadius = 50;
        [SerializeField] public float pinRadius = 5;
        [HideInInspector] public List<GPSPin> gpsPins;

        [SerializeField] public UnityEvent<GPSData> OnGPSPositionUpdated;
        [SerializeField] public UnityEvent<GPSPin> OnGPSPinInRange;

        [SerializeField] public UnityEvent<GPSPin> OnGPSPinOutOfRange;
        [SerializeField] public UnityEvent OnAllGPSPinsOutOfRange;

        [SerializeField] public UnityEvent<GPSPin> OnEnterGPSPin;
        [SerializeField] public UnityEvent<GPSPin> OnExitGPSPin;

        

        // [Space]
        // [SerializeField] public double testLatitude;
        // [SerializeField] public double testLongitude;
    }

    public class GPSData{
        public double accuracy, altitude, altitudeAccuracy, heading, latitude, longitude, speed, distanceFromOrigin, alpha;
    }

    public partial class WorldTracker
    {
        [DllImport("__Internal")] private static extern void WebGLGetGPSPosition();
        [DllImport("__Internal")] private static extern void WebGLSubscribeToGPSPositionUpdates();
        [DllImport("__Internal")] private static extern void WebGLUnsubscribeToGPSPositionUpdates();

        [SerializeField] public bool useGeolocation = false;

        [SerializeField] public GeolocationSettings geolocationSettings;
        public GPSData cameraPosData;

        private bool noPinsInRange = true;
        
        public void StartGPS(){
            if(useGeolocation){

                geolocationSettings.gpsPins = FindObjectsOfType<GPSPin>().ToList();
                Debug.Log("Found gps pins - " + geolocationSettings.gpsPins.Count);

#if UNITY_WEBGL && !UNITY_EDITOR
                WebGLSubscribeToGPSPositionUpdates();
#endif
            }

#if UNITY_EDITOR
            OnGPSPosition("0,0,0,0," + debugStartLat + "," + debugStartLon + ",0,0");
#endif

        }


        float gpsTestoffsetX = 0;
        float gpsTestoffsetZ = 0;
        [SerializeField] double debugStartLat = -27.516158;
        [SerializeField] double debugStartLon = 153.269033;

        

        public void OnDestroyGPS(){
            if(useGeolocation){
#if !UNITY_EDITOR && UNITY_WEBGL
                WebGLUnsubscribeToGPSPositionUpdates();
#endif
            }
        }

        public void OnGPSPosition(string data){
            data = data.Replace("null","0").Replace("NaN","0");
            // Debug.Log(data);
            var vals = data.Split(new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);

            cameraPosData = new GPSData{
                // accuracy         = double.Parse(vals[0], CultureInfo.InvariantCulture),
                altitude         = double.Parse(vals[1], CultureInfo.InvariantCulture),
                // altitudeAccuracy = double.Parse(vals[2], CultureInfo.InvariantCulture),
                // heading          = double.Parse(vals[3], CultureInfo.InvariantCulture),
                latitude         = double.Parse(vals[4], CultureInfo.InvariantCulture),
                longitude        = double.Parse(vals[5], CultureInfo.InvariantCulture),
                // speed            = double.Parse(vals[6], CultureInfo.InvariantCulture),
                // alpha            = double.Parse(vals[7], CultureInfo.InvariantCulture)
            };
            
            var nearbyPinCount = 0;
            foreach (var pin in geolocationSettings.gpsPins){
                //reposition GPS objects based on new coordinates

                var pos = pin.ConvertGPSToCartesian(
                    cameraPosData.latitude,
                    cameraPosData.longitude,
                    pin.altitude
                );

                pin.targetPos = pos;

                var distance = Vector3.Distance(trackerCamera.transform.position, pin.transform.position);
                // Debug.Log(pin.transform.name + " is " + distance + " m away");
                //Debug.Log(pin.name + "  " + distance + " " + pin.inRange);
                if( distance > geolocationSettings.activationRadius && pin.inRange)
                {
                    pin.inRange = false;
                    
                    Debug.Log("Pin " + pin.id + " out of range!");
                    pin.transform.gameObject.SetActive(false);
                    
                    geolocationSettings.OnGPSPinOutOfRange?.Invoke(pin);
                }
                else if(distance <= geolocationSettings.activationRadius)
                {
                    nearbyPinCount++;

                    if(!pin.inRange){
                        pin.inRange = true;

                        Debug.Log("Pin " + pin.id + " in range!");
                        pin.transform.gameObject.SetActive(true);

                        geolocationSettings.OnGPSPinInRange?.Invoke(pin);
                    }
                }
 
            }
            
            // Debug.Log("nearbyPins = " + nearbyPinCount);

            if(!noPinsInRange && nearbyPinCount <= 0){
                Debug.Log("No pins in range!");
                geolocationSettings.OnAllGPSPinsOutOfRange?.Invoke();
            }

            noPinsInRange = (nearbyPinCount <= 0);
            geolocationSettings.OnGPSPositionUpdated.Invoke(cameraPosData);
        }

        public void OnGPSPositionError(string error){
            Debug.LogError(error);
        }

        public void UpdateGPS(){
            if(useGeolocation){
                float lerpSpeed = 2.5f;
                foreach(var pin in geolocationSettings.gpsPins){
                    pin.transform.position = Vector3.Lerp(pin.transform.position, pin.targetPos, Time.deltaTime * lerpSpeed);
                
                    var distance = Vector3.Distance(pin.transform.position, trackerCamera.transform.position);
                    if(distance <= geolocationSettings.pinRadius && !pin.entered){
                        pin.entered = true;
                        geolocationSettings.OnEnterGPSPin?.Invoke(pin);
                    }
                    else if (distance > geolocationSettings.pinRadius && pin.entered){
                        pin.entered = false;
                        geolocationSettings.OnExitGPSPin?.Invoke(pin);
                    }
                }
            }
        }

        void Update_DebugGeolocation(){

            float sensitivity = 0.0002f;
            gpsTestoffsetZ += Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;
            gpsTestoffsetX += Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;
            var lat = debugStartLat + gpsTestoffsetX;
            var lon = debugStartLon + gpsTestoffsetZ;
            OnGPSPosition("0,0,0,0," + lat + "," + lon + ",0,0");

        }
    }

}
