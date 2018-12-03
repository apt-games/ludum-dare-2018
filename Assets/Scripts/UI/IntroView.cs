using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class IntroView : MonoBehaviour
{
    private CanvasGroup _group;
    private AudioSource _audio;

    public CanvasGroup Title;
    public CanvasGroup WakeUp;

    public SpeechDisplay SpeechDisplay;

    private bool _audioFinished = false;

	// Use this for initialization
	void Awake ()
	{
	    _group = GetComponent<CanvasGroup>();
	    _audio = GetComponent<AudioSource>();
	    _audio.Play();

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
	    if (!_audioFinished && _audio.time > 20.0f)
	    {
	        AudioFinished();
	    }
    }

    private void AudioFinished()
    {
        _audioFinished = true;
        SpeechDisplay.StartAnimatedTexts();
        StartCoroutine(ViewUtils.FadeOut(Title));
    }
}
