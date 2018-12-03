using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class IntroController : MonoBehaviour
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
	            StartCoroutine(FadeIn(WakeUp));
            }
	    };

	    StartCoroutine(FadeIn(Title));
	}

    public void FadeOut()
    {
        StartCoroutine(FadeOut(_group, () => { gameObject.SetActive(false); }));
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        // fade in 0.2s
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            group.alpha = t;
            yield return null;
        }
    }

    private IEnumerator FadeOut(CanvasGroup group, Action then = null)
    {
        // fade in 0.2s
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime * 2f)
        {
            group.alpha = 1-t;
            yield return null;
        }
        group.alpha = 0;
        then?.Invoke();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_audio.isPlaying)
	    {
	        SpeechDisplay.gameObject.SetActive(true);
	        StartCoroutine(FadeOut(Title));
        }
	}
}
