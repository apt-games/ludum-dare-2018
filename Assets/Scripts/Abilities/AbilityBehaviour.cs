using UnityEngine;
using UnityEngine.Events;

public class AbilityBehaviour : MonoBehaviour
{
    public UnityEvent AbilityUsed;

    public int Uses;

    public BaseAbility Ability;

    public void UseAbility()
    {
        if (Uses > 0)
        {
            Uses--;
            Debug.Log($"Using Ability {AbilityUsed != null}");
            AbilityUsed?.Invoke();
        }
    }
}
