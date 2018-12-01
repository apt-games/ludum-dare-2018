using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MapController MapController;

    public List<CharacterBehaviour> Players;

	// Use this for initialization
	void Start ()
	{
	    MapController.RoomSelected += OnRoomSelected;
	}

    private void OnRoomSelected(RoomBehaviour obj)
    {
        foreach (var player in Players)
        {
            player.MoveTo(obj.transform);
        }
    }
}
