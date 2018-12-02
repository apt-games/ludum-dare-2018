using UnityEngine;

public static class CharacterFactory
{
    public static int ID = 1;

    private static readonly string[] _characters =
    {
        "Characters/CharacterPrefab"
    };

    public static CharacterBehaviour Create(Vector3 position, Transform parent)
    {
        var character = Factory.LoadPrefab<CharacterBehaviour>(_characters[0], position, parent);

        character.ID = ID++;

        // fill with initial info
        character.CharacterInfo = CharacterInfoGenerator.getCharacterInfo();

        var ability = AbilityFactory.Create();
        character.AddAbility(ability);

        return character;
    }
}
