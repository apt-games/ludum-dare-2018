using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public event Action<RoomBehaviour> Selected;

    public GameObject VisibilityBlocker;

    public WallBehaviour[] Walls = new WallBehaviour[4];
    
    public Room Model;

    public Renderer Floor;

    public List<object> Items { get; } = new List<object>();

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

    public void AddItem(CharacterBehaviour character)
    {
        Items.Add(character);
    }

    public bool ContainsCharacter(out CharacterBehaviour character)
    {
        foreach (var item in Items)
        {
            var behaviour = item as CharacterBehaviour;
            if (behaviour == null) continue;
            character = behaviour;
            return true;
        }

        character = null;
        return false;
    }
}
