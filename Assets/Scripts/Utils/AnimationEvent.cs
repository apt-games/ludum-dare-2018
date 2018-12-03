using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent Event;

    public void TriggerEvent()
    {
        if (GameController.TrapEffectsEnabled)
            Event?.Invoke();
    }
}
