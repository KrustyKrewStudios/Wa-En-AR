using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Imagine.WebAR;

namespace Imagine.WebAR.Editor{
    [CustomEditor(typeof(GPSPin))]
    public class GPSPinEditor : UnityEditor.Editor
    {
        GPSPin _target;

        void OnEnable(){
            _target = (GPSPin)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Show in Google Maps")){
                var latitude = _target.latitude;
                var longitude = _target.longitude;
                Application.OpenURL($"https://www.google.com/maps/place/{latitude},{longitude}/@{latitude},{longitude},60m/data=!3m1!1e3?entry=ttu");
            }
        }
    }
}


