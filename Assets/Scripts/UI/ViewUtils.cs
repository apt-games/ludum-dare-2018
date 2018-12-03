using System;
using System.Collections;
using UnityEngine;

public static class ViewUtils
{
    public static IEnumerator FadeOut(CanvasGroup group, Action then = null, float after = 0)
    {
        yield return new WaitForSeconds(after);

        // fade in 0.5s
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime * 2f)
        {
            group.alpha = 1 - t;
            yield return null;
        }
        group.alpha = 0;
        then?.Invoke();
    }

    public static IEnumerator FadeIn(CanvasGroup group, Action then = null, float after = 0)
    {
        yield return new WaitForSeconds(after);

        // fade in 1s
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime * 2f)
        {
            group.alpha = t;
            yield return null;
        }
        group.alpha = 1;
        then?.Invoke();
    }
}
