using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CameraController CameraController;
    public PlayerController PlayerController;
    public MapController MapController;

    public RoomBehaviour SelectedRoom;

    public static GameController Instance => GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();

    private void Awake()
    {
        MapController.RoomSelected += OnRoomSelected;

        MapController.InitiateMap();
    }

    private void Start()
    {
        CameraController.ShowRoom(MapController.Current);
        PlayerController.Init(MapController.Current);
        MapController.Current.SetVisited(true);
    }

    private void OnRoomSelected(RoomBehaviour room)
    {
        SelectedRoom = room;

        if (_toggleAbility)
        {
            PlayerController.SelectedCharacter.UseAbility();
        }
        else
        {
            MapController.Current = room;
            CameraController.ShowRoom(room);
            room.SetVisited(true);

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
                    // TODO: Update UI here
                }
                else
                {
                    // TODO: Glow Revive ability
                }
            }
        }
    }

    private bool _toggleAbility;
    private bool _toggleAll;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            _toggleAbility = false;
            _toggleAll = false;
            SelectCharacter();
            Debug.Log($"Selected single");
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            _toggleAbility = false;
            _toggleAll = true;
            SelectCharacter();
            Debug.Log($"Selected party");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _toggleAbility = !_toggleAbility;

            Debug.Log($"Ability {_toggleAbility}");
        }
    }

    private void SelectCharacter()
    {
        // select first which is not current
        PlayerController.SelectedCharacter = PlayerController.Players.FirstOrDefault(c => c.IsAlive && c != PlayerController.SelectedCharacter);
        if (PlayerController.SelectedCharacter != null)
        {
            MapController.Current = PlayerController.SelectedCharacter.OccupyingRoom;
            CameraController.ShowRoom(PlayerController.SelectedCharacter.OccupyingRoom);
        }
    }
}
