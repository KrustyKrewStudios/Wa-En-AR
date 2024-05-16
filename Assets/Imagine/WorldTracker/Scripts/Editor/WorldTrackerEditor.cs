using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Imagine.WebAR.Editor
{
    [CustomEditor(typeof(WorldTracker))]
    public class WorldTrackerEditor : UnityEditor.Editor
    {
        WorldTracker _target;

        bool showKeyboardCameraControls = false;


        private void OnEnable()
        {
            _target = (WorldTracker)target;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            var trackerCamProp = serializedObject.FindProperty("trackerCamera");
            EditorGUILayout.PropertyField(trackerCamProp);
            var modeProp = serializedObject.FindProperty("mode");
            EditorGUILayout.PropertyField(modeProp);

            if (_target.mode == WorldTracker.TrackingMode.MODE_3DOF)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                ;
                var rssProp = serializedObject.FindProperty("s3dof");
                //rssProp.isExpanded = true;
                EditorGUILayout.PropertyField(rssProp, new GUIContent("3DOF Settings"));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            else if (_target.mode == WorldTracker.TrackingMode.MODE_3DOF_ORBIT)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                ;
                var rssProp = serializedObject.FindProperty("s3dof_orbit");
                //rssProp.isExpanded = true;
                EditorGUILayout.PropertyField(rssProp, new GUIContent("3DOF Orbit Settings"));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            else if (_target.mode == WorldTracker.TrackingMode.MODE_6DOF)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                ;
                var ttsProp = serializedObject.FindProperty("s6dof");
                //ttsProp.isExpanded = true;
                EditorGUILayout.PropertyField(ttsProp, new GUIContent("6DOF Settings"));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(20);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mainObject"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraStartHeight"));
            var useCompassProp = serializedObject.FindProperty("useCompass");
            EditorGUILayout.PropertyField(useCompassProp, new GUIContent("Use Compass (Experimental)"));
            if(useCompassProp.boolValue){
                EditorGUILayout.HelpBox("Note: Experimental compass feature has not been widely tested on all mobile browsers.\nSome browsers need up to 20 seconds to properly initialize the compass", MessageType.Warning);

                if(modeProp.intValue == (int)WorldTracker.TrackingMode.MODE_6DOF){
                    EditorGUILayout.HelpBox("UseCompass does not have an effect in 6DOF mode", MessageType.Warning);
                }
            }
            EditorGUILayout.Space(20);

            var usePlacementIndicatorProp = serializedObject.FindProperty("usePlacementIndicator");
            EditorGUILayout.PropertyField(usePlacementIndicatorProp);
            if(usePlacementIndicatorProp.boolValue){
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                ;
                var psProp = serializedObject.FindProperty("placementIndicatorSettings");
                //psProp.isExpanded = true;
                EditorGUILayout.PropertyField(psProp, new GUIContent("Placement Indicator Settings"));
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            else{
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Your main object will auto-placed");
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space(20);
            var esProp = serializedObject.FindProperty("eventSettings");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(esProp, new GUIContent("Event Settings"));
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(20);
            EditorGUI.BeginChangeCheck();
            var useGeoLocationProp = serializedObject.FindProperty("useGeolocation");
            EditorGUILayout.PropertyField(useGeoLocationProp, new GUIContent("Use Geolocation (Experimental)"));
            
            if(useGeoLocationProp.boolValue){
                if(!useCompassProp.boolValue){
                    EditorGUILayout.HelpBox("It is recommended to enable UseCompass with UseGeolocation", MessageType.Warning);
                }
                if(modeProp.intValue == (int)WorldTracker.TrackingMode.MODE_6DOF){
                    EditorGUILayout.HelpBox("Geolocation does not work properly in 6DOF mode. Use 3DOF instead!", MessageType.Warning);
                }

                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                var gsProp = serializedObject.FindProperty("geolocationSettings");
                //psProp.isExpanded = true;
                EditorGUILayout.PropertyField(gsProp, new GUIContent("Geolocation Settings"));
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            if(EditorGUI.EndChangeCheck()){
                //add remove geolocationScenes
                var scenePath = EditorSceneManager.GetActiveScene().path;
                var geolocationScenes = WorldTrackerGlobalSettings.Instance.geolocationScenes;
                if(useGeoLocationProp.boolValue){
                    //add
                    if(!geolocationScenes.Contains(scenePath))
                        geolocationScenes.Add(scenePath);
                }
                else{
                    if(geolocationScenes.Contains(scenePath))
                        geolocationScenes.Remove(scenePath);
                }
                EditorUtility.SetDirty(WorldTrackerGlobalSettings.Instance);
            }

            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debugStartLat"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debugStartLon"));
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();
            //keyboard camera controls
            showKeyboardCameraControls = EditorGUILayout.Toggle ("Show Keyboard Camera Controls", showKeyboardCameraControls);
            if(showKeyboardCameraControls){
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("W", "Move Forward (Z)");
                EditorGUILayout.LabelField("S", "Move Backward (Z)");
                EditorGUILayout.LabelField("A", "Move Left (X)");
                EditorGUILayout.LabelField("D", "Move Right (X)");
                EditorGUILayout.LabelField("R", "Move Up (Y)");
                EditorGUILayout.LabelField("F", "Move Down (Y)");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Up Arrow", "Tilt Up (along X-Axis)");
                EditorGUILayout.LabelField("Down Arrow", "Tilt Down (along X-Axis)");
                EditorGUILayout.LabelField("Left Arrow", "Tilt Left (along Y-Axis)");
                EditorGUILayout.LabelField("Right Arrow", "Tilt Right (Along Y-Axis)");
                EditorGUILayout.LabelField("Period", "Tilt Clockwise (Along Z-Axis)");
                EditorGUILayout.LabelField("Comma", "Tilt Counter Clockwise (Along Z-Axis)");
                EditorGUILayout.Space(40);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("debugCamMoveSensitivity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("debugCamTiltSensitivity"));
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();


        }

        void OnSceneGUI(){

            if(serializedObject.FindProperty("useGeolocation").boolValue){
                var radius = serializedObject.FindProperty("geolocationSettings").FindPropertyRelative("activationRadius").floatValue;
                Handles.color = new Color(1,1,0,0.05f);
                Handles.DrawSolidDisc(Vector3.zero, Vector3.up, radius);
            }
            
        }
    }
}

