// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// namespace Imagine.WebAR.Editor{
//     [CustomPropertyDrawer(typeof(GPSPin))]
//     public class GPSPinPropertyDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             EditorGUI.BeginProperty(position, label, property);

//             // Indent label and content
//             //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//             // Calculate rects
//             var nameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//             var transformRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
//             var latitudeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight);
//             var longitudeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 4, position.width, EditorGUIUtility.singleLineHeight);
//             var altitudeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 5, position.width, EditorGUIUtility.singleLineHeight);
//             var buttonRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 6, position.width, EditorGUIUtility.singleLineHeight);

//             // Find the properties
//             SerializedProperty nameProp = property.FindPropertyRelative("name");
//             SerializedProperty transformProp = property.FindPropertyRelative("transform");
//             SerializedProperty latitudeProp = property.FindPropertyRelative("latitude");
//             SerializedProperty longitudeProp = property.FindPropertyRelative("longitude");
//             SerializedProperty altitudeProp = property.FindPropertyRelative("altitude");

//             // Draw fields
//             EditorGUI.PropertyField(nameRect, nameProp);
//             EditorGUI.PropertyField(transformRect, transformProp);
//             EditorGUI.PropertyField(latitudeRect, latitudeProp);
//             EditorGUI.PropertyField(longitudeRect, longitudeProp);
//             EditorGUI.PropertyField(altitudeRect, altitudeProp);

            
            
//             if(GUI.Button(buttonRect, "View in Google Maps")){
//                 var latitude = property.FindPropertyRelative("latitude").floatValue;
//                 var longitude = property.FindPropertyRelative("longitude").floatValue;
//                 Application.OpenURL($"https://www.google.com/maps/place/{latitude},{longitude}/@{latitude},{longitude},60m/data=!3m1!1e3?entry=ttu");
//             }
            

//             EditorGUI.EndProperty();
//         }

//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             // Height of the property drawer
//             return EditorGUIUtility.singleLineHeight * 8;
//         }
//     }
// }

