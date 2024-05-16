using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Imagine.WebAR.Demo{
    [System.Serializable]
    public class TrackedPin{
        public GPSPin pin;
        [HideInInspector] public RectTransform pinMarker;
    }
    public class GPSDemo : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] RectTransform pinMarkerPrefab;
        [HideInInspector] List<TrackedPin> trackedPins = new List<TrackedPin>();

        [SerializeField] TextMeshProUGUI youReachedPinText;
        private WorldTracker wt;
        IEnumerator Start(){
            
            wt = FindObjectOfType<WorldTracker>();
            wt.geolocationSettings.OnGPSPositionUpdated.AddListener(OnGPSPositionUpdated);

            pinMarkerPrefab.gameObject.SetActive(false);
            youReachedPinText.gameObject.SetActive(false);

            //delay initialization to allo for worldtracker to create pins
            yield return new WaitForEndOfFrame();
            CreatePins();
        }

        void OnDestroy(){
            wt.geolocationSettings.OnGPSPositionUpdated.RemoveListener(OnGPSPositionUpdated);
        }
        
        void OnGPSPositionUpdated(GPSData data){
        }

        void CreatePins(){

            foreach(var pin in wt.geolocationSettings.gpsPins){

                var pinMarker = Instantiate(pinMarkerPrefab, pinMarkerPrefab.parent);

                var trackedPin = new TrackedPin{
                    pin = pin,
                    pinMarker = pinMarker
                };

                pinMarker.gameObject.SetActive(true);
                trackedPin.pinMarker = pinMarker;

                var pinText = pinMarker.GetComponentInChildren<TextMeshProUGUI>();
                pinText.text = trackedPin.pin.id;
                pinMarker.name = trackedPin.pin.id + " Pin";

                trackedPins.Add(trackedPin);
            }
        }

        void Update(){

            var container = pinMarkerPrefab.parent.GetComponent<RectTransform>();
            var canvasW = container.rect.width;
            var canvasH = container.rect.height;

            foreach(var trackedPin in trackedPins){
                var pin = trackedPin.pinMarker;

                var viewPos = cam.WorldToViewportPoint(trackedPin.pin.transform.position);

                SetPinText(pin, viewPos.x > 0.5f);

                viewPos.x -= 0.5f;
                viewPos.y -= 0.5f;

                viewPos.x = Mathf.Clamp(viewPos.x, -0.5f, 0.5f);
                //viewPos.y = Mathf.Clamp(viewPos.y, -0.5f, 0.5f);

                pin.anchoredPosition = new Vector2(viewPos.x * canvasW, viewPos.y * canvasH);
                pin.gameObject.SetActive(viewPos.z > 0.5f && trackedPin.pin.transform.gameObject.activeInHierarchy);

                var dist = Mathf.Round(trackedPin.pin.transform.position.magnitude / 5) * 5;
                
                var pinText = pin.GetComponentInChildren<TextMeshProUGUI>();
                pinText.text = trackedPin.pin.id + "\n" + dist + "m";
            }
        }

        void SetPinText(RectTransform pin, bool right = true){
            var pinText = pin.GetComponentInChildren<TextMeshProUGUI>();
            if(!right){
                pinText.rectTransform.anchoredPosition = new Vector2(10, 0);
                pinText.alignment = TextAlignmentOptions.MidlineLeft;
            }
            else {
                pinText.rectTransform.anchoredPosition = new Vector2(-430, 0);
                pinText.alignment = TextAlignmentOptions.MidlineRight;
            }

            //toggle pin to force reactivate changes
            pinText.gameObject.SetActive(false);
            pinText.gameObject.SetActive(true);
        }

        public void OnEnterPin(GPSPin pin){
            youReachedPinText.gameObject.SetActive(true);
            youReachedPinText.text = "You reached Pin - " + pin.id + "!";
        }

        public void OnExitPin(GPSPin pin){
            youReachedPinText.gameObject.SetActive(false);
        }
    }

}

