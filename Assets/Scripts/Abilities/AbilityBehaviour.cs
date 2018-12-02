using UnityEngine;
using UnityEngine.Events;

public class AbilityBehaviour : MonoBehaviour
{
    public UnityEvent AbilityUsed;

    public int Uses;

    public void UseAbility()
    {
        if (Uses >= 0)
        {
            Debug.Log("Using Ability");
            AbilityUsed?.Invoke();
        }
    }
}
