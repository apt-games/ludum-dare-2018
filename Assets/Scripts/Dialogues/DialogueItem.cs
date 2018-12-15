public struct DialogueItem
{
    public readonly int Character;
    public readonly string Text;
    public readonly float WaitBefore;
    public readonly float Duration;

    public DialogueItem(int character, string text, float waitBefore = 1, float showDuration = 2)
    {
        Character = character;
        Text = text;
        WaitBefore = waitBefore;
        Duration = showDuration;
    }
}
