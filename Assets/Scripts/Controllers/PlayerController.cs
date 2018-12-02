using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public List<CharacterBehaviour> Players { get; } = new List<CharacterBehaviour>();

    public CharacterBehaviour SelectedCharacter;

    public void Init(RoomBehaviour room)
    {
        Spawn(room);
    }

    private void Spawn(RoomBehaviour room)
    {
        var position = new Vector3(room.transform.position.x, room.transform.position.y, transform.position.z);
        var initialCharacter = CharacterFactory.CreateInitial(position, transform);
        initialCharacter.OccupyingRoom = room;
        Debug.Log("Spawned at " + position);
        SelectedCharacter = initialCharacter;
        Players.Add(initialCharacter);

        var secondChar = CharacterFactory.CreateInitial(new Vector3(position.x - 0.1f, position.y, position.z), transform);
        secondChar.OccupyingRoom = room;
        Players.Add(secondChar);
    }

    public void MoveSelectedCharacterTo(RoomBehaviour room)
    {
        SelectedCharacter?.MoveTo(room);
    }

    public void MovePartyTo(RoomBehaviour room)
    {
        foreach (var player in Players.Where(player => player.IsAlive))
        {
            player.MoveTo(room);
        }
    }

    public void KillCharacter(CharacterBehaviour character)
    {
        Debug.Log("Killing char");
        character.Die();
        SelectedCharacter = null;
    }
}
