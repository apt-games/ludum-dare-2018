using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent Event;

    public void TriggerEvent()
    {
        Event?.Invoke();
    }
}
