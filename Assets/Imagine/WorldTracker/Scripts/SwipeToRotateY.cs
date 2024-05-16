using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR
{
    public class SwipeToRotateY : MonoBehaviour
    {

        [SerializeField] private Transform rotTransform;
        //TODO: implement using Screen.DPI / Canvas.innerWidth.Height API grabber

        private Vector2 startDragPos;
        private bool isDragging = false;



        private Quaternion origRot, startRot;

        [SerializeField] private float sensitivity = 20;

        void Start()
        {
            origRot = rotTransform.rotation;
        }

        void Update()
        {
            if (Input.touchCount > 1)
            {
                isDragging = false;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                startDragPos = Input.mousePosition;
                startRot = rotTransform.rotation;
                isDragging = true;
            }

            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                var curDragPos = Input.mousePosition;

                var x = curDragPos.x - startDragPos.x;
                var a = x * sensitivity * -1;

                rotTransform.rotation = startRot * Quaternion.AngleAxis(a, Vector3.up);

            }

        }

        public void Reset()
        {
            rotTransform.rotation = origRot;
        }
    }
}
