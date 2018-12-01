using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<CharacterBehaviour> Players;


    public void MoveAllPlayersTo(RoomBehaviour room)
    {
        foreach (var player in Players)
        {
            player.MoveTo(room.transform);
        }
    }
}
