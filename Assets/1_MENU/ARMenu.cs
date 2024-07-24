using Imagine.WebAR;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMenu : MonoBehaviour
{
    public GameObject arCanvas;
    public ARCamera arCamera;
    public GameObject uiCanvas;

    public GameObject searedWagyu;
    public GameObject beefPlatter;
    public GameObject tata;
    public GameObject wagyuJyu;
    public GameObject salad;

    public GameObject debugPanel;


    public WorldTracker worldTracker;
    public GameObject placeButton;

    public void ToggleDebugger()
    {
        // Check if the debug panel is active
        bool isActive = debugPanel.activeSelf;

        // Toggle the active state
        debugPanel.SetActive(!isActive);
        Debug.Log("Toggle ui");

    }

    void Start()
    {
        arCamera = GetComponent<ARCamera>();
        worldTracker = GetComponent<WorldTracker>();
        if (worldTracker == null)
        {
            worldTracker = FindObjectOfType<WorldTracker>();
            if (worldTracker == null)
            {
                Debug.LogError("WorldTracker component not found in the scene.");
            }
            else
            {
                Debug.Log("WorldTracker component found.");
                worldTracker.StopTracker();
            }
        }
    }


    public void StopTrakcer()
    {
        worldTracker.StopTracker();
        Debug.Log("stop the tracker bij");
    }

    public void StartTracker()
    {
        worldTracker.StartTracker();
        placeButton.SetActive(true);
        Debug.Log("start the tracker now bij");
    }

    public void ArMode()
    {
        arCamera.UnpauseCamera();
        arCanvas.SetActive(true);
        uiCanvas.SetActive(false);

    }
        
    public void NoAr()
    {
        arCamera.PauseCamera();
        arCanvas.SetActive(false);
        uiCanvas.SetActive(true);
    }

    public void DisableAR()
    {
        arCanvas.SetActive(false);

    }

    public void OffUI()
    {
        uiCanvas.SetActive(false);
        Debug.Log("off ui");
    }

    public void OnUI()
    {
        uiCanvas.SetActive(true);
    }

    public void SetSalad()
    {
        StartTracker();
        salad.SetActive(true);
        wagyuJyu.SetActive(false);
        tata.SetActive(false);
        beefPlatter.SetActive(false);
        searedWagyu.SetActive(false);
        uiCanvas.SetActive(false );


    }

    public void SetWagyuJyu() 
    {
        StartTracker();
        wagyuJyu.SetActive(true);
        tata.SetActive(false);
        beefPlatter.SetActive(false);
        searedWagyu.SetActive(false);
        salad.SetActive(false);
        uiCanvas.SetActive(false);
    }

    public void SetTata()
    {
        StartTracker();
        tata.SetActive(true);
        beefPlatter.SetActive(false);
        searedWagyu.SetActive(false);
        salad.SetActive(false);
        wagyuJyu.SetActive(false);
        uiCanvas.SetActive(false);
    }

    public void SetPlatter()
    {
        StartTracker();
        beefPlatter.SetActive(true);
        searedWagyu.SetActive(false);
        salad.SetActive(false);
        wagyuJyu.SetActive(false);
        tata.SetActive(false);
        uiCanvas.SetActive(false);
        worldTracker.StartTracker();
        arCanvas.SetActive(true);

    }

    public void SetSearedWagyu()
    {
        StartTracker();
        searedWagyu.SetActive(true);
        salad.SetActive(false);
        wagyuJyu.SetActive(false);
        tata.SetActive(false);
        beefPlatter.SetActive(false);
        uiCanvas.SetActive(false);

    }


}
