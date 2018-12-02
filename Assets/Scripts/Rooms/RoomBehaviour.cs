using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public event Action<RoomBehaviour> Selected;

    public SpriteRenderer VisibilityBlocker;

    public WallBehaviour[] Walls = new WallBehaviour[4];
    
    public Room Model;

    public Renderer Floor;

    public Color SemiVisibleColor = Color.gray;

    public List<object> Items { get; } = new List<object>();

    public RoomVisibilityStatus Visibility { get; private set; } = RoomVisibilityStatus.Hidden;

    private void Awake()
    {
        VisibilityBlocker.gameObject.SetActive(true);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Selected?.Invoke(this);
        }
    }

    public void SetDiscovered()
    {
        if (Visibility == RoomVisibilityStatus.Hidden)
        {
            Visibility = RoomVisibilityStatus.Discovered;
            StartCoroutine(AnimateColorToSemiVisible());
        }
    }

    public IEnumerator AnimateColorToSemiVisible()
    {
        float ElapsedTime = 0.0f;
        float TotalTime = 0.5f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            VisibilityBlocker.color = Color.Lerp(Color.black, SemiVisibleColor, (ElapsedTime / TotalTime));
            yield return null;
        }
    }

    public void SetVisited()
    {
        if (Visibility != RoomVisibilityStatus.Visited)
        {
            Visibility = RoomVisibilityStatus.Visited;
            VisibilityBlocker.gameObject.SetActive(false);
        }
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

    public void AddItem(CharacterBehaviour character)
    {
        Items.Add(character);
    }

    public bool ContainsCharacter(out CharacterBehaviour character)
    {
        character = null;

        foreach (var item in Items)
        {
            var behaviour = item as CharacterBehaviour;
            if (behaviour == null) continue;
            character = behaviour;
        }

        if (character != null)
        {
            Items.Remove(character);
            return true;
        }
        return false;
    }
}

public enum RoomVisibilityStatus
{
    Hidden, Discovered, Visited,
}
