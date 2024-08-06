using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VideoHere : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string[] videoFilenames; // Array to hold your video filenames
    private int currentVideoIndex = 0;

    void Start()
    {
        // Optionally initialize or leave empty if you only want to start via button
    }

    public void StartVideoSequence()
    {
        if (videoFilenames.Length > 0)
        {
            currentVideoIndex = 0; // Reset to start with the first video
            PlayVideo(videoFilenames[currentVideoIndex]);
        }
        else
        {
            Debug.LogError("No video files specified.");
        }
    }

    void PlayVideo(string videoFilename)
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFilename);
        videoPlayer.url = videoPath;
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.errorReceived += OnVideoError;
        videoPlayer.loopPointReached += OnVideoEnd;

        videoPlayer.Prepare();
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Stop listening to the current video end event
        videoPlayer.loopPointReached -= OnVideoEnd;

        // Move to the next video
        currentVideoIndex = (currentVideoIndex + 1) % videoFilenames.Length;
        PlayVideo(videoFilenames[currentVideoIndex]);
    }

    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError($"VideoPlayer Error: {message}");
    }
}
