using System;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public event Action<RoomBehaviour> Selected;

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("clicked "+name);
            Selected?.Invoke(this);
        }
    }
}
