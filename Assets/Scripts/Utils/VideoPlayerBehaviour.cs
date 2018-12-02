using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerBehaviour : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

	void Awake ()
	{
	    _videoPlayer = GetComponent<VideoPlayer>();
	    _videoPlayer.seekCompleted += OnCompleted;

        _videoPlayer.url = Application.streamingAssetsPath + "/splash.mp4";

        _videoPlayer.Play();
	}

    private void OnCompleted(VideoPlayer source)
    {
        SceneManager.LoadSceneAsync("Main");
    }
}
