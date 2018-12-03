using System;
using Assets.Scripts.MapGenerator;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    public Map CurrentMap { get; private set; }

    public RoomBehaviour CurrentRoom => CurrentMap?.Current;


    public void InitiateLevel0()
    {
        //TODO make tutorial map    
        //CurrentMap = new Map(MapGenerator.CreateGeneratedMap(12), 12, transform);
    }

    public void InitiateLevel1()
    {
        if (CurrentMap != null)
        {
            CurrentMap.RoomSelected -= RoomSelected;
            CurrentMap.gameObject.SetActive(false);
            Destroy(CurrentMap.gameObject);
        }

        CurrentMap = new Map(MapGenerator.CreateGeneratedMap(12), 12, transform);
        CurrentMap.RoomSelected += RoomSelected;
    }

    public void SelectRoom(RoomBehaviour room)
    {
        CurrentMap.SetCurrentRoom(room);
    }
}
