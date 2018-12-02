using UnityEngine;

public static class CharacterFactory
{
    private static readonly string[] _characters =
    {
        "Characters/CharacterPrefab"
    };

    public static CharacterBehaviour Create()
    {
        var character = Factory.LoadPrefab<CharacterBehaviour>("Characters/CharacterPrefab");

        // fill with random info
        character.CharacterInfo = CharacterInfoGenerator.getCharacterInfo();

        var ability = AbilityFactory.Create();
        character.AddAbility(ability);

        return character;
    }

    public static CharacterBehaviour CreateInitial(Vector3 position, Transform parent)
    {
        var character = Factory.LoadPrefab<CharacterBehaviour>(_characters[0], position, parent);

        // fill with initial info
        character.CharacterInfo = CharacterInfoGenerator.getCharacterInfo();

        var ability = AbilityFactory.Create();
        character.AddAbility(ability);

        return character;
    }
}
