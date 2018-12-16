using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<CharacterBehaviour> Characters { get; } = new List<CharacterBehaviour>();
    public event Action<CharacterBehaviour> CharacterAdded;
    public event Action<CharacterBehaviour> CharacterDied;

    public CharacterBehaviour SelectedCharacter;
    public AudioSource newCharAudio;
    public static bool HasWokenFirstCharacter = false;

    public void Init()
    {
        HasWokenFirstCharacter = false;
        var initialCharacter = CharacterFactory.Create(Vector3.zero, transform);
        Characters.Add(initialCharacter);

        CharacterAdded?.Invoke(initialCharacter);
    }

    public void PlaceCharactersInRoom(RoomBehaviour room)
    {
        var position = new Vector3(room.transform.position.x, room.transform.position.y, transform.position.z);

        foreach (var character in Characters)
        {
            character.TeleportToWorldPositions(position);
            character.OccupyingRoom = room;
        }
    }

    public void AddToParty(CharacterBehaviour character)
    {
        character.transform.SetParent(transform);
        Characters.Add(character);
        newCharAudio.PlayDelayed(1.0f);

        CharacterAdded?.Invoke(character);
    }

    public void MoveSelectedCharacterTo(RoomBehaviour room)
    {
        if (SelectedCharacter != null)
        {
            SelectedCharacter.MoveTo(room);

            var currentlyInParty = Characters.Where(player => player.IsAlive && (player.ID != SelectedCharacter?.ID)).ToList();
            if (room.Visibility == RoomVisibilityStatus.Visited && room.IsSafe)
            {
                MovePartyTo(room, currentlyInParty);
            }
            else
            {
                //TODO: Improve this
                StartCoroutine(WaitForSafe(room, currentlyInParty));
            }
        }
    }

    private IEnumerator WaitForSafe(RoomBehaviour room, List<CharacterBehaviour> party)
    {
        if (room != null)
        {
            yield return new WaitForSeconds(room.IsSafe ? 1.5f : 3f);
            MovePartyTo(room, party);
        }
    }

    public void MovePartyTo(RoomBehaviour room, List<CharacterBehaviour> party)
    {
        foreach (var player in party)
        {
            player.MoveTo(room);
        }
    }

    public void KillCharacter(CharacterBehaviour character)
    {
        character.Die();

        // if a part of party
        if (Characters.Contains(character))
        {
            SelectedCharacter = null;
            CharacterDied?.Invoke(character);
        }
    }
}
