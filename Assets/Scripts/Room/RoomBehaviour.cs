using System;
using UnityEngine;
using UnityEngine.AI;

public class RoomBehaviour : MonoBehaviour
{
    public event Action<RoomBehaviour> Selected;

    public GameObject VisibilityBlocker;

    public WallBehaviour[] Walls = new WallBehaviour[4];
    
    public Room Model;

    public Renderer Floor;

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

        if (room.type == RoomType.Death)
            Floor.material.color = Color.red;
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
