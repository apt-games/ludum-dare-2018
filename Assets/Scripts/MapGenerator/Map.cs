using System;
using System.Collections.Generic;
using Assets.Scripts.Factories;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MapGenerator
{
    public class Map
    {
        public event Action<RoomBehaviour> RoomSelected;

        public RoomBehaviour Current { get; private set; }

        private readonly List<RoomBehaviour> _rooms = new List<RoomBehaviour>();

        private readonly int Size;

        // Use this for initialization
        public Map(IEnumerable<Room> rooms, int size, Transform parent)
        {
            Size = size;

            var mapGameObject = new GameObject($"Map ({size}, {size})");
            mapGameObject.transform.SetParent(parent, false);
            mapGameObject.transform.localPosition = Vector3.zero;

            foreach (var room in rooms)
            {
                // spawn tiles as children
                var spawned = RoomFactory.Create(room);
                spawned.transform.parent = mapGameObject.transform;
                spawned.Selected += OnRoomClicked;

                _rooms.Add(spawned);

                // start room
                if (room.type == RoomType.Start)
                    Current = spawned;
            }

            SetCurrentRoom(Current);

            // add surface mesh on current object and build from this
            var surface = Current.Floor.gameObject.AddComponent<NavMeshSurface>();
            surface.BuildNavMesh();

            RoomFactory.PlaceItems(_rooms);
        }

        public void SetCurrentRoom(RoomBehaviour room)
        {
            Current = room;
            Current.SetVisited();
            ShowNeighbours(room);
        }

        private void OnRoomClicked(RoomBehaviour room)
        {
            if (CanMoveToRoom(room.Model))
            {
                RoomSelected?.Invoke(room);
            }
        }

        private void ShowNeighbours(RoomBehaviour room)
        {
            var i = _rooms.IndexOf(room);
            var walls = room.Model.walls;

            for (int d = 0; d < walls.Length; d++)
            {
                if (walls[d] == 0) //dor
                {
                    switch (d)
                    {
                        //top
                        case 0:
                            _rooms[i - Size].SetDiscovered();
                            break;
                        //right
                        case 1:
                            _rooms[i + 1].SetDiscovered();
                            break;
                        //bottom
                        case 2:
                            _rooms[i + Size].SetDiscovered();
                            break;
                        //left
                        case 3:
                            _rooms[i - 1].SetDiscovered();
                            break;
                        default:
                            Debug.LogError("Wah? this shouldn't happen, should only have 4 walls");
                            break;
                    }
                }
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
    }
}
