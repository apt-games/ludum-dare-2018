using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class IntroView : MonoBehaviour
{
    private CanvasGroup _group;
    private AudioSource _audio;

    public CanvasGroup Title;
    public CanvasGroup WakeUp;

    public SpeechDisplay SpeechDisplay;

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
	    if (_audio.time > 26.0f)
	    {
	        SpeechDisplay.gameObject.SetActive(true);
	        StartCoroutine(ViewUtils.FadeOut(Title));
        }
    }
}
