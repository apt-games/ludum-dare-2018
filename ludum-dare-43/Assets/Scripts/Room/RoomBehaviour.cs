using System;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public event Action<RoomBehaviour> Selected;

    public GameObject VisibilityBlocker;

    public WallBehaviour[] Walls = new WallBehaviour[4];
    
    public Room Model;

    private void Awake()
    {
        SetVisited(false);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("clicked " + name);
            Selected?.Invoke(this);
        }
    }

    public void SetVisited(bool visited)
    {
        VisibilityBlocker.SetActive(!visited);
    }

    public void SetModel(Room room)
    {
        Model = room;

        SetWalls(room.walls);
    }

    private void SetWalls(int[] roomWalls)
    {
        for (int i = 0; i < roomWalls.Length; i++)
        {
            if (roomWalls[i] == 0)
                Walls[i].SetIsDoor();
        }
    }
}