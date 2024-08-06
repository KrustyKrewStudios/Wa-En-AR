using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI; // Make sure to include this for UI elements

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private bool isPrepared = false;

    void Start()
    {
        // Set up the video path and prepare the video player
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "video.mp4");
        videoPlayer.url = videoPath;

        Debug.Log($"Video path set to: {videoPath}");

        videoPlayer.prepareCompleted += OnVideoPrepared;

        // Prepare the video player but don't start playing yet
        Debug.Log("Preparing video player...");
        videoPlayer.Prepare();


    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        isPrepared = true;
        Debug.Log("Video prepared. Ready to play.");
    }

    public void PlayVideo()
    {
        // Start playing the video only if it is prepared
        if (isPrepared)
        {
            Debug.Log("Playing video...");
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Video is not yet prepared.");
        }
    }
}
