using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<CharacterBehaviour> Characters { get; } = new List<CharacterBehaviour>();
    public event Action PlayersChanged;

    public CharacterBehaviour SelectedCharacter;
    public AudioSource newCharAudio;

    public void Init()
    {
        var initialCharacter = CharacterFactory.Create(Vector3.zero, transform);
        Characters.Add(initialCharacter);

        var secondChar = CharacterFactory.Create(new Vector3(- 0.2f, 0, 0), transform);
        Characters.Add(secondChar);

        PlayersChanged?.Invoke();
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

        PlayersChanged?.Invoke();
    }

    public void MoveSelectedCharacterTo(RoomBehaviour room)
    {
        if (SelectedCharacter != null)
        {
            SelectedCharacter.MoveTo(room);

            if (room.Visibility != RoomVisibilityStatus.Visited && room.IsSafe)
            {
                //TODO: Improve this
                StartCoroutine(WaitForSafe(room));
            }
            else
            {
                MovePartyTo(room);
            }
        }
    }

    private IEnumerator WaitForSafe(RoomBehaviour room)
    {
        yield return new WaitForSeconds(3);
        if (room != null)
            MovePartyTo(room);
    }

    public void MovePartyTo(RoomBehaviour room)
    {
        foreach (var player in Characters.Where(player => player.IsAlive && (SelectedCharacter != null && player.ID != SelectedCharacter.ID)))
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
            PlayersChanged?.Invoke();
        }
    }
}
