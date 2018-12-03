using System;
using Assets.Scripts.MapGenerator;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    private Map _currentMap;
    public Map CurrentMap
    {
        get { return _currentMap; }
        private set
        {
            if (_currentMap != null)
            {
                _currentMap.gameObject.SetActive(false);
                _currentMap.RoomSelected -= RoomSelected;
            }
            _currentMap = value;
            _currentMap.RoomSelected += RoomSelected;
        }
    }

    public RoomBehaviour CurrentRoom => CurrentMap?.Current;


    public void InitiateLevel0()
    {
        //TODO make tutorial map    
        //CurrentMap = new Map(MapGenerator.CreateGeneratedMap(12), 12, transform);
    }

    public void InitiateLevel1()
    {
        CurrentMap = new Map(MapGenerator.CreateGeneratedMap(12), 12, transform);
    }

    public void SelectRoom(RoomBehaviour room)
    {
        CurrentMap.SetCurrentRoom(room);
    }
}
