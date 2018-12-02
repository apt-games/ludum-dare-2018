﻿using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent Event;

    public void TriggerEvent()
    {
        Event?.Invoke();
    }
}
