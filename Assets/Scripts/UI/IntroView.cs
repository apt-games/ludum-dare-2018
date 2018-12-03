using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class IntroView : MonoBehaviour
{
    private CanvasGroup _group;
    public AudioSource ZurichAudio;
    public AudioSource SpeechDisplayAudio;

    public CanvasGroup Title;
    public CanvasGroup WakeUp;

    public SpeechDisplay SpeechDisplay;

    private bool _audioFinished = false;

	// Use this for initialization
	void Awake ()
	{
	    _group = GetComponent<CanvasGroup>();
	    ZurichAudio.Play();

	    _group.alpha = 1;

	    SpeechDisplay.Completed += () =>
	    {
            WakeUp.gameObject.SetActive(true);
	        {
	            StartCoroutine(ViewUtils.FadeIn(WakeUp));
            }
	    };

	    StartCoroutine(ViewUtils.FadeIn(Title));
    }

    public void FadeOut()
    {
        StartCoroutine(ViewUtils.FadeOut(_group, () => { gameObject.SetActive(false); }));
    }

	// Update is called once per frame
	void Update ()
	{
	    if (!_audioFinished && ZurichAudio.time > 20.0f)
	    {
	        AudioFinished();
	    }
    }

    private void AudioFinished()
    {
        SpeechDisplayAudio.Play();
        _audioFinished = true;
        SpeechDisplay.StartAnimatedTexts();
        StartCoroutine(ViewUtils.FadeOut(Title));
    }
}
