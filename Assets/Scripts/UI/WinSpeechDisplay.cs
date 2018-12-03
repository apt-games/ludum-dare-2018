using System;
using UnityEngine;

public class WinSpeechDisplay : SpeechDisplay
{
    [Header("Limit for decent feedback (seconds)")]
    public int NormalThreshold = 360;
    [Header("Limit for good feedback (seconds)")]
    public int GoodThreshold = 240;

    [Space(10), Header("Feedback")]
    public string BadFeedbad = "Below average, but still finished!";
    public string NormalFeedback = "Pretty decent.";
    public string GoodFeedback = "Fastest so far!";

    protected override string ParseText(string text)
    {
        return text.Replace("{time}", GetCurrentTimeText());
    }

    private string GetCurrentTimeText()
    {
        var time = DateTime.Now - GameController.LevelStarted;

        var t = $"{time.Minutes} minutes and {time.Seconds} seconds. ";

        if (time.TotalSeconds <= GoodThreshold)
            return t + GoodFeedback;
        if (time.TotalSeconds <= NormalThreshold)
            return t + NormalFeedback;
        return t + BadFeedbad;
    }
}
