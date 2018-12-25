using System;
using UnityEngine;

[Serializable]
public class DialogueLine 
{
    public PersonalityTrait CharacterTraitsNeeded = PersonalityTrait.None;
    public string Text = string.Empty;
    public float WaitBefore = 1;
    public float Duration = 2;
}
