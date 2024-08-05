using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Set up the video path and prepare the video player
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "side-eye_1.mp4");
        videoPlayer.url = videoPath;
        videoPlayer.prepareCompleted += OnVideoPrepared;

        // Prepare the video player but don't start playing yet
        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
        // Lock screen orientation to landscape when starting the video
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Start playing the video
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            // If the video is not yet prepared, start playing after preparation is complete
            videoPlayer.prepareCompleted += OnVideoPrepared;
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // Start playing the video
        vp.Play();
    }
}
