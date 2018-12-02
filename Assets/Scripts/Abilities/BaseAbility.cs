using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public AbilityType Type;

    public virtual void Use()
    {

    }
}

public enum AbilityType
{
    Flare, Stone, Clairvoyance, Revive
}
