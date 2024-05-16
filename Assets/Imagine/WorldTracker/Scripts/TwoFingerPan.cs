using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR
{
    public class TwoFingerPan : MonoBehaviour
    {
        [SerializeField] Transform panTransform;
        [SerializeField] Transform cam;
        Vector3 origPos, startPos;
        Vector2 touch0StartPos, touch1StartPos;

        [SerializeField] float sensitivity = 0.01f;

        bool isPanning = false;

        void Start()
        {
            Input.multiTouchEnabled = true;
            origPos = panTransform.localPosition;
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
                    isPanning = true;
                    startPos = panTransform.localPosition;

                    Debug.Log("Start Pan");
                }
                else if (touch0.phase == TouchPhase.Ended ||
                    touch1.phase == TouchPhase.Ended ||
                    touch0.phase == TouchPhase.Canceled ||
                    touch1.phase == TouchPhase.Canceled)
                {
                    isPanning = false;
                    Debug.Log("End Pan");
                }

                if (isPanning)
                {
                    var centerStart = (touch1StartPos + touch0StartPos) / 2;

                    var pos0 = touch0.position;
                    var pos1 = touch1.position;
                    var centerEnd = (pos1 + pos0) / 2;

                    var offset = centerEnd - centerStart;

                    var worldOffset = cam.right * offset.x + cam.up * offset.y;

                    panTransform.localPosition = startPos + worldOffset * sensitivity * -1;

                    //Debug.Log("Move " + offset + "->" + worldOffset);
                }


            }
            else
            {
                isPanning = false;
            }

        }

        public void Reset()
        {
            panTransform.localPosition = origPos;
        }
    }
}
