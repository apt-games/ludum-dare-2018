using UnityEngine;

public static class AbilityFactory
{
    private static readonly string[] _abilities =
    {
        "Abilities/FlareAbility",
    };

    public static AbilityBehaviour Create()
    {
        var ability = Factory.LoadPrefab<AbilityBehaviour>(_abilities[Random.Range(0, _abilities.Length)]);

        // fill with random info

        return ability;
    }
}
