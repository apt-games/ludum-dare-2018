using System;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    public int Height = 10;
    public int Width = 10;

    public RoomBehaviour RoomPrefab;

    // Use this for initialization
    private void Start ()
	{
	    var midX = Width / 2;
	    var midY = Height / 2;

	    for (var y = -midY; y < midY; y++)
	    {
	        for (var x = -midX; x < midX; x++)
	        {
                // spawn tiles as children
	            var spawned = Instantiate(RoomPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
	            spawned.name = $"Room ({x},{y})";
	            spawned.Selected += OnRoomClicked;
	        }
        }
	}

    private void OnRoomClicked(RoomBehaviour room)
    {
        RoomSelected?.Invoke(room);
    }
}
