using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR
{
    public class PinchToScale : MonoBehaviour
    {
        public void SetScriptEnabled()
        {
            this.enabled = true;
        }

        public void SetScriptDisabled()
        {
            this.enabled = false;
        }

        [SerializeField] Transform scaleTransform;
        Vector3 origScale, startScale;
        Vector2 touch0StartPos, touch1StartPos;

        [SerializeField] float minScale = 0.1f;
        [SerializeField] float maxScale = 10f;

        bool isPinching = false;


        void Awake(){
            origScale = scaleTransform.localScale;
        }
        
        void Start()
        {
            Input.multiTouchEnabled = true;
            
        }

        void Update()
        {
            if (Input.touchCount == 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Began ||
                    touch1.phase == TouchPhase.Began)
                {
                    touch0StartPos = touch0.position;
                    touch1StartPos = touch1.position;
                    isPinching = true;
                    startScale = scaleTransform.localScale;

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

                    var scale = Mathf.Clamp(d / dStart, minScale, maxScale);
                    scaleTransform.localScale = startScale * scale;

                    //Debug.Log("Pinch " + scale.ToString("0.00") + "x");
                }


            }
            else
            {
                isPinching = false;
            }

        }

        public void Reset()
        {
            scaleTransform.localScale = origScale;
        }
    }
}
