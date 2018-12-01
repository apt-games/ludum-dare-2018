using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MapController MapController;
    public CameraController CameraController;

    public List<CharacterBehaviour> Players;

	private void Start ()
	{
	    MapController.RoomSelected += OnRoomSelected;
	}

    private void OnRoomSelected(RoomBehaviour room)
    {
        CameraController.ShowRoom(room.transform);

        foreach (var player in Players)
        {
            player.MoveTo(room.transform);
        }
    }
}
