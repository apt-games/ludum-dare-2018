using UnityEngine;
using UnityEngine.Events;

public class TrapEffectBehaviour : MonoBehaviour
{
    public TrapType Type;

    public UnityEvent TriggerEffectEvent;

    public int Uses = 1;

    public float Delay = 0.2f;
}
