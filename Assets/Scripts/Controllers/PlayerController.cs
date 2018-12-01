using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public List<CharacterBehaviour> Players;

    private void Awake()
    {

    }

    public void MoveAllPlayersTo(RoomBehaviour room)
    {
        foreach (var player in Players)
        {
            player.MoveTo(room.transform);
        }
    }
}
