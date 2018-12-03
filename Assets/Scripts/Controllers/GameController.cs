using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static bool TrapEffectsEnabled { get; private set; } = false;

    public CameraController CameraController;
    public PlayerController PlayerController;
    public MapController MapController;
    public UIController UIController;

    public UnityEvent OnPlayIntro;
    public UnityEvent OnStartTutorial;
    public UnityEvent OnStartNormal;
    public UnityEvent OnPlayerDied;

    [HideInInspector]
    public RoomBehaviour SelectedRoom;

    public static GameController Instance => GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();

    private void Awake()
    {
        PlayerController.PlayersChanged += OnPlayersChanged;
        MapController.RoomSelected += OnRoomSelected;

        RestartPlayer();

        StartIntro();
    }

    public void RestartPlayer()
    {
        foreach (var child in PlayerController.Characters)
            Destroy(child.gameObject);
        PlayerController.Characters.Clear();

        PlayerController.Init();
    }

    public void StartIntro()
    {
        // play intro
        OnPlayIntro?.Invoke();
    }

    public void StartTutorial()
    {
        TrapEffectsEnabled = false;
        MapController.InitiateLevel1(); // TODO: replace with 0 when tutorial 
        PlayerController.PlaceCharactersInRoom(MapController.CurrentRoom);
        CameraController.ShowRoom(MapController.CurrentRoom);

        OnStartTutorial?.Invoke();
    }

    public void StartNormal()
    {
        TrapEffectsEnabled = false;
        MapController.InitiateLevel1();
        PlayerController.PlaceCharactersInRoom(MapController.CurrentRoom);
        CameraController.ShowRoom(MapController.CurrentRoom);

        OnStartNormal?.Invoke();
    }

    private void OnRoomSelected(RoomBehaviour room)
    {
        if (!TrapEffectsEnabled)
            TrapEffectsEnabled = true;

        if (PlayerController.SelectedCharacter == null || PlayerController.SelectedCharacter.IsWalking) {
            return;
        }

        SelectedRoom = room;

        if (_toggleAbility)
        {
            PlayerController.SelectedCharacter.UseAbility();
            SelectCharacter(null);
        }
        else
        {
            MapController.SelectRoom(room);
            CameraController.ShowRoom(room);

            MapController.SelectRoom(room);

            if (_toggleAll)
                PlayerController.MovePartyTo(room);
            else
                PlayerController.MoveSelectedCharacterTo(room);

            CharacterBehaviour character;

            if (room.ContainsCharacter(out character))
            {
                if (character.IsAlive)
                {
                    PlayerController.AddToParty(character);
                }
                else
                {
                    // TODO: Glow Revive ability
                }
            }
        }

        SetAbilityActive(false);

        UIController.UpdateUI();
    }

    private bool _toggleAbility;
    private bool _toggleAll;

    private void Update()
    {
    }

    public void SelectCharacter(CharacterBehaviour character)
    {
        // select first which is not current
        PlayerController.SelectedCharacter = character;
        SetAbilityActive(false);

        if (PlayerController.SelectedCharacter != null)
        {
            MapController.SelectRoom(PlayerController.SelectedCharacter.OccupyingRoom);
            CameraController.ShowRoom(PlayerController.SelectedCharacter.OccupyingRoom);
        }
    }

    public void SetAbilityActive (bool state) {
        _toggleAbility = state;
    }

    public void OnPlayersChanged()
    {
        UIController?.UpdateUI();

        if (!PlayerController.Characters.Any(p => p.IsAlive))
            OnPlayerDied?.Invoke();
    }
}
