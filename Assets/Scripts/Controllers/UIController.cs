using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour {
    private int _characterAvatarHeight = 128;
    private int _characterAvatarMargin = 20;
    private int _characterAvatarPosX {get; set;}
    private int _initialCharacterAvatarPosY {get; set;}
    private CharacterAvatar _activeCharacterAvatar {get; set;}
    private CharacterAbility _activeCharacterAbility {get; set;}
    private readonly List<CharacterAvatar> _characterAvatars = new List<CharacterAvatar>();

    public GameObject Content;
    public PlayerController PlayerController;
    public CharacterAvatar CharacterAvatarPrefab;

    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.ForceSoftware;

    private Texture2D _defaultCursor;
    private Texture2D _walkCursor;

    public UIController() {
        _characterAvatarPosX = (_characterAvatarHeight / 2) - _characterAvatarMargin;
        _initialCharacterAvatarPosY = (_characterAvatarPosX * -1) + _characterAvatarMargin;
    }

    public void Awake() {
        _defaultCursor = Resources.Load<Texture2D>("UI/cursors/cursor");
        _walkCursor = Resources.Load<Texture2D>("UI/cursors/walk cursor");

        Cursor.SetCursor(_defaultCursor, hotSpot, cursorMode);
    }

	// Use this for initialization
	public void UpdateUI () {
        _activeCharacterAbility = null;

        foreach (var characterAvatar in _characterAvatars) {
            Destroy(characterAvatar.gameObject);
        }

        _characterAvatars.Clear();

        int posY = _initialCharacterAvatarPosY;
        bool stillHaveActiveCharacterAvatar = false;

        foreach (var Character in PlayerController.Players.Where(p => p.IsAlive)) {
            Vector3 position = new Vector3(_characterAvatarPosX, posY, 0);

            var characterAvatar = Instantiate(CharacterAvatarPrefab, Vector3.zero, Quaternion.identity, Content.transform);

            characterAvatar.gameObject.SetActive(true);
            characterAvatar.Character = Character;
            RectTransform transform = (RectTransform)  characterAvatar.transform;

            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, _characterAvatarPosX, transform.rect.width);
            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -posY, transform.rect.height);

            // characterAvatar.transform.localPosition = position;

            var ImageComponent = characterAvatar.Image.GetComponent<Image>();
            ImageComponent.sprite = Character.CharacterInfo.avatar;
            TextMeshProUGUI avatarName = characterAvatar.AvatarName.GetComponent<TextMeshProUGUI>();
            avatarName.SetText(Character.CharacterInfo.name);

            characterAvatar.AvatarClicked += OnAvatarClick;
            characterAvatar.CharacterAbilityClicked += OnAbilityClick;

            characterAvatar.CreateActions();

            _characterAvatars.Add(characterAvatar);

            if (_activeCharacterAvatar != null && _activeCharacterAvatar.Character.ID == Character.ID) {
                stillHaveActiveCharacterAvatar = true;
                characterAvatar._animator.SetFloat("animationspeed", 1000f);
                characterAvatar.SetSelected(true);
                _activeCharacterAvatar = characterAvatar;
            }

            posY = posY - (_characterAvatarHeight + _characterAvatarMargin);
        }

        if (_activeCharacterAvatar == null || !stillHaveActiveCharacterAvatar) {
            Cursor.SetCursor(_defaultCursor, hotSpot, cursorMode);
        }
    }

    public void OnAvatarClick (CharacterAvatar CharacterAvatar) {

        if (_activeCharacterAvatar != null) {
            if (CharacterAvatar == _activeCharacterAvatar) {
                _activeCharacterAvatar = null;
                CharacterAvatar.SetSelected(false);
                GameController.Instance.SelectCharacter(null);
                Cursor.SetCursor(_defaultCursor, hotSpot, cursorMode);
                return;
            }

            Debug.Log(_activeCharacterAvatar);
            _activeCharacterAvatar.SetSelected(false);
        }

        if (_activeCharacterAbility != null) {
            _activeCharacterAbility.SetSelected(false);
            _activeCharacterAbility = null;
        }

        CharacterAvatar._animator.SetFloat("animationspeed", 1f);
        CharacterAvatar.SetSelected(true);
        _activeCharacterAvatar = CharacterAvatar;
        GameController.Instance.SelectCharacter(CharacterAvatar.Character);
        Cursor.SetCursor(_walkCursor, hotSpot, cursorMode);
    }

    public void ResetSelection() {
        if (_activeCharacterAbility != null) {
            _activeCharacterAbility.SetSelected(false);
            _activeCharacterAbility = null;
            GameController.Instance.SetAbilityActive(false);
        }

        if (_activeCharacterAvatar != null) {
            GameController.Instance.SelectCharacter(null);
            _activeCharacterAvatar.SetSelected(false);
            _activeCharacterAvatar = null;
        }

        Cursor.SetCursor(_defaultCursor, hotSpot, cursorMode);
    }

    public void OnAbilityClick (CharacterAbility characterAbility) {
        if (_activeCharacterAbility == null || characterAbility != _activeCharacterAbility) {
            if (_activeCharacterAbility != null) {
                _activeCharacterAbility.SetSelected(false);
            }

            _activeCharacterAbility = characterAbility;
            characterAbility.SetSelected(true);

            GameController.Instance.SelectCharacter(characterAbility.CharacterAvatar.Character);
            GameController.Instance.SetAbilityActive(true);
        } else {
            _activeCharacterAbility.SetSelected(false);
            _activeCharacterAbility = null;
            GameController.Instance.SetAbilityActive(false);
            GameController.Instance.SelectCharacter(null);
            Cursor.SetCursor(_defaultCursor, hotSpot, cursorMode);
        }

        if (_activeCharacterAvatar != null && _activeCharacterAbility != null) {
            _activeCharacterAvatar.SetSelected(false);
            _activeCharacterAvatar = null;
        }

        if (_activeCharacterAbility != null) {
            Cursor.SetCursor(_activeCharacterAbility.texture, new Vector2(16, 16), cursorMode);
        }
    }

    public void Update() {
        if (Input.GetMouseButtonDown(1) && (_activeCharacterAvatar != null || _activeCharacterAbility)) {
            ResetSelection();
        }
    }
}