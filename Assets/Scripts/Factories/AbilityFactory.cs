using UnityEngine;

public static class AbilityFactory
{
    private static readonly string[] _abilities =
    {
        "Abilities/FlareAbility",
        "Abilities/StoneAbility",
    };

    public static AbilityBehaviour Create()
    {
        var ability = Factory.LoadPrefab<AbilityBehaviour>(_abilities[Random.Range(0, _abilities.Length)]);

        // add more random uses for ability
        ability.Uses = Random.Range(0, 3);

        // fill with random info

        return ability;
    }
}
