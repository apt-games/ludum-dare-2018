using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class WinView : MonoBehaviour
{
    private CanvasGroup _group;

    public SpeechDisplay Speech;

    public float FadeInDelay = 3f;
    public float SpeechStartDelay = 2f;

    public GameObject PlayAgain;

    // Update is called once per frame
    void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        FadeIn();

        Speech.Completed += () => { PlayAgain.gameObject.SetActive(true); };
    }

    // Use this for initialization
    public void FadeIn()
    {
        StartCoroutine(ViewUtils.FadeIn(_group, after: FadeInDelay));

        StartCoroutine(StartSpeechAfter(FadeInDelay + SpeechStartDelay));
    }

    private IEnumerator StartSpeechAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Speech.StartAnimatedTexts();
    }

    // Use this for initialization
    public void FadeOut()
    {
        StartCoroutine(ViewUtils.FadeOut(_group, () => { gameObject.SetActive(false); }));
    }
}
