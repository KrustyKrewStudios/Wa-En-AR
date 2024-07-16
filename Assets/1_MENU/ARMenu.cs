using Imagine.WebAR;
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

    void Start()
    {
       arCamera = GetComponent<ARCamera>();
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

    public void SetSalad()
    {

    }

    public void SetWagyuJyu() 
    {
    }
    
    public void SetTata()
    {

    }

    public void SetPlatter()
    {

    }

    public void SetSearedWagyu()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
