using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class StartView : MonoBehaviour
{
    private CanvasGroup _group;

    public float InitialDelay = 1f;

    void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }

    public void DisplayFor(float seconds = 2f)
    {
        StartCoroutine(ViewUtils.FadeIn(_group, () =>
            {
                StartCoroutine(ViewUtils.FadeOut(_group, after: seconds));
            },
            InitialDelay)
        );
    }
}
