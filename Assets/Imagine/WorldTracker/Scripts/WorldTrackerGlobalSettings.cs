using  System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR{
    public class WorldTrackerGlobalSettings : ScriptableObject
    {
        [SerializeField] public List<string> geolocationScenes;
        
        
        private static WorldTrackerGlobalSettings _instance;
        public static WorldTrackerGlobalSettings Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Resources.Load<WorldTrackerGlobalSettings>("WorldTrackerGlobalSettings");
                }
                return _instance;

            }
        }
    }
}

