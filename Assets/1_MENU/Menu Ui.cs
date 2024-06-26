using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class MenuUi : MonoBehaviour
{
    //Declarations
    public VideoPlayer videoPlayer;
    public GameObject animScreen;
    public GameObject homeScreen;

    public void PlayVideo()
    {
        // Play the video
        videoPlayer.Play();
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        // Deactivate the current panel and activate the next panel
        animScreen.SetActive(false);
        homeScreen.SetActive(true);
    }
}
