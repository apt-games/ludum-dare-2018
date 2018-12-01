using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    public RoomBehaviour RoomPrefab;

    public RoomBehaviour Current;

    private readonly List<RoomBehaviour> _rooms = new List<RoomBehaviour>();

    // Use this for initialization
    private void Start()
    {
        var map = JsonUtility.FromJson<MapStructure>(json);

        foreach (var room in map.Rooms)
        {
            // spawn tiles as children
            var spawned = CreateRoom(room);
            spawned.Selected += OnRoomClicked;

            _rooms.Add(spawned);
        }

        // initial;
        WalkToRoom(_rooms[0]);
    }

    private RoomBehaviour CreateRoom(Room room)
    {
        var go = Instantiate(RoomPrefab, new Vector3(room.x, -room.y, 0), Quaternion.identity, transform);
        go.name = $"Room ({room.x},{room.y})";
        go.SetModel(room);

        return go;
    }

    private void OnRoomClicked(RoomBehaviour room)
    {
        if (CanMoveToRoom(room.Model))
        {
            WalkToRoom(room);
        }
    }

    private void WalkToRoom(RoomBehaviour room)
    {
        Current = room;
        RoomSelected?.Invoke(room);
        room.SetVisited(true);
    }

    private bool CanMoveToRoom(Room room)
    {
        // check if is neighbour and open room
        var diff = new Vector2Int(room.x - Current.Model.x, room.y - Current.Model.y);

        // move up
        if (diff.x == 0 && diff.y == -1 && Current.Model.walls[0] == 0)
            return true;

        // move right
        if (diff.x == 1 && diff.y == 0 && Current.Model.walls[1] == 0)
            return true;

        // move down
        if (diff.x == 0 && diff.y == 1 && Current.Model.walls[2] == 0)
            return true;

        // move left
        if (diff.x == -1 && diff.y == 0 && Current.Model.walls[3] == 0)
            return true;

        return false;
    }

    private string json = "{\"Rooms\":[{\"x\":0,\"y\":0,\"walls\":[1,1,0,1]},{\"x\":1,\"y\":0,\"walls\":[1,0,0,1]},{\"x\":2,\"y\":0,\"walls\":[1,0,1,0]},{\"x\":3,\"y\":0,\"walls\":[1,0,1,0]},{\"x\":4,\"y\":0,\"walls\":[1,1,0,0]},{\"x\":5,\"y\":0,\"walls\":[1,1,0,1]},{\"x\":6,\"y\":0,\"walls\":[1,0,0,1]},{\"x\":7,\"y\":0,\"walls\":[1,0,1,0]},{\"x\":8,\"y\":0,\"walls\":[1,0,0,0]},{\"x\":9,\"y\":0,\"walls\":[1,1,0,0]},{\"x\":0,\"y\":1,\"walls\":[0,0,1,1]},{\"x\":1,\"y\":1,\"walls\":[0,1,1,0]},{\"x\":2,\"y\":1,\"walls\":[1,0,0,1]},{\"x\":3,\"y\":1,\"walls\":[1,0,1,0]},{\"x\":4,\"y\":1,\"walls\":[0,1,1,0]},{\"x\":5,\"y\":1,\"walls\":[0,0,0,1]},{\"x\":6,\"y\":1,\"walls\":[0,0,1,0]},{\"x\":7,\"y\":1,\"walls\":[1,1,1,0]},{\"x\":8,\"y\":1,\"walls\":[0,1,0,1]},{\"x\":9,\"y\":1,\"walls\":[0,1,0,1]},{\"x\":0,\"y\":2,\"walls\":[1,0,0,1]},{\"x\":1,\"y\":2,\"walls\":[1,1,1,0]},{\"x\":2,\"y\":2,\"walls\":[0,1,0,1]},{\"x\":3,\"y\":2,\"walls\":[1,1,0,1]},{\"x\":4,\"y\":2,\"walls\":[1,0,0,1]},{\"x\":5,\"y\":2,\"walls\":[0,1,1,0]},{\"x\":6,\"y\":2,\"walls\":[1,0,0,1]},{\"x\":7,\"y\":2,\"walls\":[1,0,1,0]},{\"x\":8,\"y\":2,\"walls\":[0,1,1,0]},{\"x\":9,\"y\":2,\"walls\":[0,1,0,1]},{\"x\":0,\"y\":3,\"walls\":[0,1,0,1]},{\"x\":1,\"y\":3,\"walls\":[1,0,0,1]},{\"x\":2,\"y\":3,\"walls\":[0,1,1,0]},{\"x\":3,\"y\":3,\"walls\":[0,0,1,1]},{\"x\":4,\"y\":3,\"walls\":[0,1,1,0]},{\"x\":5,\"y\":3,\"walls\":[1,0,0,1]},{\"x\":6,\"y\":3,\"walls\":[0,1,1,0]},{\"x\":7,\"y\":3,\"walls\":[1,0,0,1]},{\"x\":8,\"y\":3,\"walls\":[1,1,1,0]},{\"x\":9,\"y\":3,\"walls\":[0,1,0,1]},{\"x\":0,\"y\":4,\"walls\":[0,1,0,1]},{\"x\":1,\"y\":4,\"walls\":[0,0,1,1]},{\"x\":2,\"y\":4,\"walls\":[1,1,0,0]},{\"x\":3,\"y\":4,\"walls\":[1,0,0,1]},{\"x\":4,\"y\":4,\"walls\":[1,0,1,0]},{\"x\":5,\"y\":4,\"walls\":[0,1,1,0]},{\"x\":6,\"y\":4,\"walls\":[1,0,0,1]},{\"x\":7,\"y\":4,\"walls\":[0,0,1,0]},{\"x\":8,\"y\":4,\"walls\":[1,1,0,0]},{\"x\":9,\"y\":4,\"walls\":[0,1,0,1]},{\"x\":0,\"y\":5,\"walls\":[0,0,0,1]},{\"x\":1,\"y\":5,\"walls\":[1,1,1,0]},{\"x\":2,\"y\":5,\"walls\":[0,1,0,1]},{\"x\":3,\"y\":5,\"walls\":[0,0,0,1]},{\"x\":4,\"y\":5,\"walls\":[1,0,1,0]},{\"x\":5,\"y\":5,\"walls\":[1,1,0,0]},{\"x\":6,\"y\":5,\"walls\":[0,1,0,1]},{\"x\":7,\"y\":5,\"walls\":[1,1,0,1]},{\"x\":8,\"y\":5,\"walls\":[0,1,0,1]},{\"x\":9,\"y\":5,\"walls\":[0,1,0,1]},{\"x\":0,\"y\":6,\"walls\":[0,0,0,1]},{\"x\":1,\"y\":6,\"walls\":[1,0,1,0]},{\"x\":2,\"y\":6,\"walls\":[0,1,1,0]},{\"x\":3,\"y\":6,\"walls\":[0,1,0,1]},{\"x\":4,\"y\":6,\"walls\":[1,0,0,1]},{\"x\":5,\"y\":6,\"walls\":[0,1,1,0]},{\"x\":6,\"y\":6,\"walls\":[0,1,0,1]},{\"x\":7,\"y\":6,\"walls\":[0,1,0,1]},{\"x\":8,\"y\":6,\"walls\":[0,0,1,1]},{\"x\":9,\"y\":6,\"walls\":[0,1,1,0]},{\"x\":0,\"y\":7,\"walls\":[0,1,0,1]},{\"x\":1,\"y\":7,\"walls\":[1,0,1,1]},{\"x\":2,\"y\":7,\"walls\":[1,0,1,0]},{\"x\":3,\"y\":7,\"walls\":[0,1,1,0]},{\"x\":4,\"y\":7,\"walls\":[0,0,1,1]},{\"x\":5,\"y\":7,\"walls\":[1,1,0,0]},{\"x\":6,\"y\":7,\"walls\":[0,1,0,1]},{\"x\":7,\"y\":7,\"walls\":[0,0,0,1]},{\"x\":8,\"y\":7,\"walls\":[1,0,1,0]},{\"x\":9,\"y\":7,\"walls\":[1,1,0,0]},{\"x\":0,\"y\":8,\"walls\":[0,0,1,1]},{\"x\":1,\"y\":8,\"walls\":[1,1,0,0]},{\"x\":2,\"y\":8,\"walls\":[1,0,0,1]},{\"x\":3,\"y\":8,\"walls\":[1,0,1,0]},{\"x\":4,\"y\":8,\"walls\":[1,0,1,0]},{\"x\":5,\"y\":8,\"walls\":[0,1,1,0]},{\"x\":6,\"y\":8,\"walls\":[0,0,1,1]},{\"x\":7,\"y\":8,\"walls\":[0,1,1,0]},{\"x\":8,\"y\":8,\"walls\":[1,1,0,1]},{\"x\":9,\"y\":8,\"walls\":[0,1,0,1]},{\"x\":0,\"y\":9,\"walls\":[1,0,1,1]},{\"x\":1,\"y\":9,\"walls\":[0,0,1,0]},{\"x\":2,\"y\":9,\"walls\":[0,1,1,0]},{\"x\":3,\"y\":9,\"walls\":[1,0,1,1]},{\"x\":4,\"y\":9,\"walls\":[1,0,1,0]},{\"x\":5,\"y\":9,\"walls\":[1,0,1,0]},{\"x\":6,\"y\":9,\"walls\":[1,0,1,0]},{\"x\":7,\"y\":9,\"walls\":[1,0,1,0]},{\"x\":8,\"y\":9,\"walls\":[0,0,1,0]},{\"x\":9,\"y\":9,\"walls\":[0,1,1,0]}]}";
}

public class MapStructure
{
    public Room[] Rooms;
}
