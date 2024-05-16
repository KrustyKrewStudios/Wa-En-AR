using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR.Demo{

    public class CompassUI : MonoBehaviour
    {
       [SerializeField] Transform cam;
       [SerializeField] RectTransform pointer;
        void Update()
        {
            if(cam != null && pointer != null){
                pointer.localEulerAngles = new Vector3(0, 0, -cam.eulerAngles.y);
            }
        }
    }
}
