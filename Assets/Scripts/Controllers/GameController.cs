using UnityEngine;

public class GameController : MonoBehaviour
{
    public CameraController CameraController;
    public PlayerController PlayerController;
    public MapController MapController;

    private void Awake()
    {
        MapController.RoomSelected += OnRoomSelected;

        MapController.InitiateMap();
    }

    private void Start()
    {
        CameraController.ShowRoom(MapController.Current);
        PlayerController.Init(MapController.Current);
        MapController.Current.SetVisited(true);
    }

    private void OnRoomSelected(RoomBehaviour room)
    {
        WalkToRoom(room);
    }

    private void WalkToRoom(RoomBehaviour room)
    {
        MapController.Current = room;
        CameraController.ShowRoom(room);
        PlayerController.MoveAllPlayersTo(room);

        room.SetVisited(true);
    }
}
