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
            WalkToRoom(room);
        }
    }

    private void WalkToRoom(RoomBehaviour room)
    {
        MapController.Current = room;
        CameraController.ShowRoom(room);
        PlayerController.MoveAllPlayersTo(room);

        room.SetVisited(true);
    }

    private bool _toggleAbility;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            _toggleAbility = !_toggleAbility;

            Debug.Log($"Toggled: {(_toggleAbility ? "Ability" : "Character")}");
        }
    }
}
