using TMPro;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class DeathView : MonoBehaviour
{
    private CanvasGroup _group;

    public float FadeInDelay = 3f;
    public TextMeshProUGUI Text;

    public string[] DeathTexts;

    // Update is called once per frame
    void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        FadeIn();
    }

    // Use this for initialization
    public void FadeIn ()
    {
        if (DeathTexts != null)
            Text.text = DeathTexts[Random.Range(0, DeathTexts.Length)];
        StartCoroutine(ViewUtils.FadeIn(_group, after: FadeInDelay));
    }

    // Use this for initialization
    public void FadeOut ()
	{
	    StartCoroutine(ViewUtils.FadeOut(_group, () => { gameObject.SetActive(false); }));
    }
}
