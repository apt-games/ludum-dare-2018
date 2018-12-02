using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Factories
{
    public static class RoomFactory
    {
        private static readonly string[] _floors = {
            "Rooms/floor03",
            "Rooms/floor08",
            "Rooms/floor07",
            "Rooms/floor01",
            "Rooms/floor02",
            "Rooms/floor04",
            "Rooms/floor05",
            "Rooms/floor06",
        };

        public static RoomBehaviour CreateRoom(global::Room model)
        {
            var roomBehaviour = Factory.LoadPrefab<RoomBehaviour>("Rooms/RoomPrefab");
            roomBehaviour.transform.position = new Vector3(model.x, -model.y, 0);
            roomBehaviour.name = $"Room ({model.x},{model.y})";
            roomBehaviour.SetModel(model);

            Debug.Log(model.type);

            switch (model.type)
            {
                case RoomType.Blocked:
                    break;
                case RoomType.Start:
                case RoomType.Exit:
                    roomBehaviour.Floor.material = Resources.Load<Material>(_floors[0]);
                    break;
                case RoomType.Safe:
                    roomBehaviour.Floor.material = Resources.Load<Material>(_floors[1]);
                    roomBehaviour.Floor.transform.Rotate(Vector3.up, Factory.RandomRotation);
                    break;
                case RoomType.UncertainSafe:
                case RoomType.UncertainDeath:
                    roomBehaviour.Floor.material = Resources.Load<Material>(_floors[Random.Range(3, _floors.Length)]);
                    roomBehaviour.Floor.transform.Rotate(Vector3.up, Factory.RandomRotation);
                    break;
                case RoomType.Death:
                    roomBehaviour.Floor.material = Resources.Load<Material>(_floors[2]);
                    roomBehaviour.Floor.transform.Rotate(Vector3.up, 180);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (model.type)
            {
                case RoomType.Blocked:
                case RoomType.Start:
                case RoomType.Exit:
                case RoomType.Safe:
                case RoomType.UncertainSafe:
                    break;
                case RoomType.UncertainDeath:
                case RoomType.Death:
                    var trap = TrapFactory.CreateTrap();
                    trap.transform.SetParent(roomBehaviour.transform, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return roomBehaviour;
        }
    }
}
