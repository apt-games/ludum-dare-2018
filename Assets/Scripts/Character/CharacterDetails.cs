using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDetails", menuName = "Character/CharacterDetails", order = 1)]
public class CharacterDetails : ScriptableObject
{
    public Sprite Avatar;

    public Sex Sex;

    public CharacterColors Colors;
}

public enum Sex
{
    Male, Female,
}
