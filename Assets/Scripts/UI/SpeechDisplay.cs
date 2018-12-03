using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class SpeechDisplay : MonoBehaviour
{
    public event Action Completed;
    public float Speed = 1f;

    public TextMeshProUGUI Left;
    public TextMeshProUGUI Right;

    public string[] Texts;

    private List<TextMeshProUGUI> _bubbles;
    private VerticalLayoutGroup _layout;

    void Awake()
    {
        _bubbles = CreateBubbles(Texts);
        _layout = GetComponent<VerticalLayoutGroup>();
    }

	// Use this for initialization
	void Start ()
	{
	    StartAnimatedTexts();
	}

    public void StartAnimatedTexts()
    {
        StartCoroutine(AnimateBubble(0));
    }

    private IEnumerator AnimateBubble(int i)
    {
        if (i > 0)
            StartCoroutine(FadeOut(_bubbles[i - 1]));

        var c = _bubbles[i];
        var l = _bubbles[i].text.Length;

        // fade in 0.2s
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime * Speed)
        {
            c.alpha = t;
            yield return null;
        }
        // display for a while
        var s = Mathf.Max(0.5f, (Mathf.Min(2f, l / 100f))) / Speed;
        yield return new WaitForSeconds(s);
        
        var from = transform.localPosition;

        if (i < _bubbles.Count - 1)
        {
            // animate up and show next
            for (var t = 0.0f; t < 1.0f; t += Time.deltaTime * 2f * Speed)
            {
                var d = (t * (_layout.spacing + _bubbles[i].rectTransform.sizeDelta.y));
                var p = new Vector3(
                    from.x,
                    from.y + d,
                    from.z);
                transform.localPosition = p;
                yield return null;
            }
            StartCoroutine(AnimateBubble(++i));
        }
        else
        {
            Completed?.Invoke();
        }
    }

    private IEnumerator FadeOut(TextMeshProUGUI text)
    {
        // fade out
        for (var t = 0.0f; t < 0.8f; t += Time.deltaTime * Speed)
        {
            text.alpha = 1-t;
            yield return null;
        }
    }

    public List<TextMeshProUGUI> CreateBubbles(string[] texts)
    {
        var list = new List<TextMeshProUGUI>();

        for (int i = 0; i < texts.Length; i++)
        {
            var left = i % 2 == 0;

            var go = Instantiate(left ? Left : Right, transform);
            go.alpha = 0;
            go.text = texts[i];

            list.Add(go);
        }

        return list;
    }
}
