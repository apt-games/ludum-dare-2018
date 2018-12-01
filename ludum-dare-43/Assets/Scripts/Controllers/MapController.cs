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
    public void InitiateMap()
    {
        var map = JsonUtility.FromJson<MapStructure>(json);

        foreach (var room in map.Rooms)
        {
            // spawn tiles as children
            var spawned = CreateRoom(room);
            spawned.Selected += OnRoomClicked;

            _rooms.Add(spawned);

            // start room
            if (room.start)
                Current = spawned;
        }
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
            RoomSelected?.Invoke(room);
        }
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

    private string json =
        "{\"Rooms\":[{\"x\":0,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,1],\"type\":1},{\"x\":1,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":2,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,1,0],\"type\":0},{\"x\":3,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,1,1,0],\"type\":0},{\"x\":4,\"y\":0,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":5,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,1,1,1],\"type\":1},{\"x\":0,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[0,0,0,1],\"type\":1},{\"x\":1,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":2,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":1},{\"x\":3,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":4,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":5,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":0,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[0,0,0,1],\"type\":0},{\"x\":1,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":1},{\"x\":2,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":1},{\"x\":3,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":4,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":1},{\"x\":5,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":1},{\"x\":0,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":1},{\"x\":1,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":2,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":0},{\"x\":3,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":1},{\"x\":4,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":0},{\"x\":5,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":0},{\"x\":0,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,1],\"type\":1},{\"x\":1,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":2,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,0],\"type\":0},{\"x\":3,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":1},{\"x\":4,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,0],\"type\":0},{\"x\":5,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":1},{\"x\":0,\"y\":5,\"start\":true,\"blocked\":false,\"walls\":[0,1,1,1],\"type\":1},{\"x\":1,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":0},{\"x\":2,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,1,1,0],\"type\":0},{\"x\":3,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[1,0,1,1],\"type\":0},{\"x\":4,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":5,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,1,1,0],\"type\":0}]}";
}

public class MapStructure
{
    public Room[] Rooms;
}
