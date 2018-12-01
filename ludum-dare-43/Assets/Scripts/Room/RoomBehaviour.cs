using System;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public event Action<RoomBehaviour> Selected;

    public WallBehaviour[] Walls = new WallBehaviour[4]; 

    private void Start()
    {
        SetVisible(false);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("clicked " + name);
            Selected?.Invoke(this);
        }
    }

    public void SetVisible(bool visible)
    {
    }

    public void SettWalls(int[] roomWalls)
    {
        for (int i = 0; i < roomWalls.Length; i++)
        {
            if (roomWalls[i] == 0)
                Walls[i].SetIsDoor();
        }
    }
}