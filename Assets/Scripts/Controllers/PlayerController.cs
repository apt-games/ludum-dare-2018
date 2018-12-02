using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public CharacterBehaviour CharacterPrefab;

    public List<CharacterBehaviour> Players { get; } = new List<CharacterBehaviour>();

    public void Init(RoomBehaviour room)
    {
        Spawn(room);
    }

    private void Spawn(RoomBehaviour room)
    {
        var position = new Vector3(room.transform.position.x, room.transform.position.y, transform.position.z);
        var player = Instantiate(CharacterPrefab, position, Quaternion.identity, transform);
        Debug.Log("Spawned at " + position);
        Players.Add(player);
    }

    public void MoveAllPlayersTo(RoomBehaviour room)
    {
        foreach (var player in Players.Where(player => player.IsAlive))
        {
            player.MoveTo(room.transform);
        }
    }

    public void KillCharacter(CharacterBehaviour character)
    {
        Debug.Log("Killing char");
        character.Die();
    }
}
