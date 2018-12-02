using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class TrapFactory
{
    private static readonly string[] _traps =
    {
        "Traps/BladeTrapEffect",
        "Traps/ElectroTrapEffect",
        "Traps/GasTrapEffect",
        "Traps/FlameTrapEffect",
    };

    public static TrapBehaviour Create()
    {
        var trapBehaviour = Factory.LoadPrefab<TrapBehaviour>("Traps/TrapPrefab");

        var effect = RandomEffect;

        switch (effect.Type)
        {
            case TrapType.Flamethrower:
                effect.transform.Rotate(new Vector3(0, 0, 1), 45);
                break;
            case TrapType.Sawblade:
            case TrapType.Gas:
            case TrapType.Electric:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        effect.transform.Rotate(new Vector3(0, 0, 1), Factory.RandomRotation);

        trapBehaviour.SetEffect(effect);

        return trapBehaviour;
    }

    public static TrapEffectBehaviour RandomEffect =>
        Factory.LoadPrefab<TrapEffectBehaviour>(_traps[Random.Range(0, _traps.Length)]);
}
