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

        WalkToRoom(MapController.Current);
    }

    private void OnRoomSelected(RoomBehaviour room)
    {
        WalkToRoom(room);
    }

    private void WalkToRoom(RoomBehaviour room)
    {
        MapController.Current = room;

        CameraController.ShowRoom(room.transform);

        PlayerController.MoveAllPlayersTo(room);

        room.SetVisited(true);
    }
}
