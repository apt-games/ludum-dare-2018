using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool TrapEffectsEnabled { get; private set; } = false;

    public CameraController CameraController;
    public PlayerController PlayerController;
    public MapController MapController;
    public UIController UIController;

    [HideInInspector]
    public RoomBehaviour SelectedRoom;

    public static GameController Instance => GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();

    private void Awake()
    {
        PlayerController.PlayersChanged += OnPlayersChanged;
        MapController.RoomSelected += OnRoomSelected;
    }

    private void Start()
    {
        StartLevel1();
    }

    private void StartLevel1()
    {
        MapController.InitiateMap();
        CameraController.ShowRoom(MapController.Current);
        PlayerController.Init(MapController.Current);
        MapController.Current.SetVisited();
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
            MapController.SetCurrentRoom(room);
            CameraController.ShowRoom(room);

            MapController.SetCurrentRoom(room);

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
            MapController.SetCurrentRoom(PlayerController.SelectedCharacter.OccupyingRoom);
            CameraController.ShowRoom(PlayerController.SelectedCharacter.OccupyingRoom);
        }
    }

    public void SetAbilityActive (bool state) {
        _toggleAbility = state;
    }

    public void OnPlayersChanged()
    {
        UIController?.UpdateUI();
    }
}
