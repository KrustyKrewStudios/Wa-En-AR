using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Set up the video path and prepare the video player
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "christmas_vid.mp4");
        videoPlayer.url = videoPath;
        videoPlayer.prepareCompleted += OnVideoPrepared;

        // Prepare the video player but don't start playing yet
        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
            videoPlayer.Play();

        }
    }

    public void PauseVideo()
    {
        // Pause the video
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer.isPlaying || videoPlayer.isPaused)
        {
            videoPlayer.Stop();
        }

    }

    public void ResumeVideo()
    {
        if (videoPlayer.isPaused)
        {
            videoPlayer.Play();
        }
    }



    void OnVideoPrepared(VideoPlayer vp)
    {
        // Start playing the video
        vp.Play();
    }
}
