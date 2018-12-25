using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueLine> DialogueLines;

    public List<PersonalityTrait> GetPersonalitiesNeeded()
    {
        List<PersonalityTrait> traitsNeeded = new List<PersonalityTrait>();
        foreach(var line in DialogueLines)
        {
            if(!traitsNeeded.Contains(line.CharacterTraitsNeeded))
            {
                traitsNeeded.Add(line.CharacterTraitsNeeded);
            }
        }
        return traitsNeeded;
    }
}
