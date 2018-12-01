using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public CharacterBehaviour CharacterPrefab;

    public List<CharacterBehaviour> Players { get; } = new List<CharacterBehaviour>();

    public void Init(RoomBehaviour room)
    {
        transform.position = new Vector3(room.transform.position.x, room.transform.position.y, transform.position.z);
        Spawn();
    }

    private void Spawn()
    {
        var player = Instantiate(CharacterPrefab, transform);
        Players.Add(player);
    }

    public void MoveAllPlayersTo(RoomBehaviour room)
    {
        foreach (var player in Players)
        {
            player.MoveTo(room.transform);
        }
    }
}
