using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR{
    public class GPSPin : MonoBehaviour{
        [SerializeField] public string id;
        [SerializeField] public double latitude = 0;
        [SerializeField] public double longitude = 0;
        [SerializeField] public double altitude = 0;

        [HideInInspector] public Vector3 targetPos;
        [HideInInspector] public bool inRange = false;
        [HideInInspector] public bool entered = false;

        public Vector3 ConvertGPSToCartesian(double referenceLatitude, double referenceLongitude, double referenceAltitude)
        {            
            // Radius of the Earth (in meters) for a simplified flat Earth model
            double earthRadius = 6371000.0;

            // Convert latitude and longitude to radians
            double latRad = latitude * Mathf.Deg2Rad;
            double lonRad = longitude * Mathf.Deg2Rad;

            // Calculate the meters per degree for latitude and longitude
            double metersPerDegreeLatitude = 111132.92 - 559.82 * Mathf.Cos(2 * (float)latRad) + 1.175 * Mathf.Cos(4 * (float)latRad) - 0.0023 * Mathf.Cos(6 * (float)latRad);
            double metersPerDegreeLongitude = Mathf.PI * earthRadius * Mathf.Cos((float)latRad) / 180.0;

            // Calculate X, Y, Z coordinates
            double z = (latitude - referenceLatitude) * metersPerDegreeLatitude;
            double y = altitude - referenceAltitude;
            double x = (longitude - referenceLongitude) * metersPerDegreeLongitude;

            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}

