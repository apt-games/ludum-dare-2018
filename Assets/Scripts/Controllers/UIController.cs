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


    public UIController() {
        _characterAvatarPosX = (_characterAvatarHeight / 2) - _characterAvatarMargin;
        _initialCharacterAvatarPosY = (_characterAvatarPosX * -1) + _characterAvatarMargin;
    }

	// Use this for initialization
	public void UpdateUI () {
        _activeCharacterAvatar = null;
        _activeCharacterAbility = null;

        foreach (var characterAvatar in _characterAvatars) {
            Destroy(characterAvatar.gameObject);
        }

        _characterAvatars.Clear();

        int posY = _initialCharacterAvatarPosY;

        foreach (var Player in PlayerController.Players.Where(p => p.IsAlive)) {
            Vector3 position = new Vector3(_characterAvatarPosX, posY, 0);


            Debug.Log(position);

            var characterAvatar = Instantiate(CharacterAvatarPrefab, Vector3.zero, Quaternion.identity, Content.transform);

            characterAvatar.gameObject.SetActive(true);
            characterAvatar.Character = Player;
            RectTransform transform = (RectTransform)  characterAvatar.transform;

            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, _characterAvatarPosX, transform.rect.width);
            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -posY, transform.rect.height);

            // characterAvatar.transform.localPosition = position;

            var ImageComponent = characterAvatar.Image.GetComponent<Image>();
            ImageComponent.sprite = Player.CharacterInfo.avatar;
            TextMeshProUGUI avatarName = characterAvatar.AvatarName.GetComponent<TextMeshProUGUI>();
            avatarName.SetText(Player.CharacterInfo.name);

            characterAvatar.AvatarClicked += OnAvatarClick;
            characterAvatar.CharacterAbilityClicked += OnAbilityClick;

            characterAvatar.CreateActions();

            _characterAvatars.Add(characterAvatar);

            posY = posY - (_characterAvatarHeight + _characterAvatarMargin);
        }

    }

    public void OnAvatarClick (CharacterAvatar CharacterAvatar) {

        if (_activeCharacterAvatar != null) {
            if (CharacterAvatar == _activeCharacterAvatar) {
                _activeCharacterAvatar = null;
                CharacterAvatar.SetSelected(false);
                GameController.Instance.SelectCharacter(null);
                return;
            }

            Debug.Log(_activeCharacterAvatar);
            _activeCharacterAvatar.SetSelected(false);
        }

        if (_activeCharacterAbility != null) {
            _activeCharacterAbility.SetSelected(false);
            _activeCharacterAbility = null;
        }

        CharacterAvatar.SetSelected(true);
        _activeCharacterAvatar = CharacterAvatar;
        GameController.Instance.SelectCharacter(CharacterAvatar.Character);
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
        }

        if (_activeCharacterAvatar != null && _activeCharacterAbility != null) {
            _activeCharacterAvatar.SetSelected(false);
            _activeCharacterAvatar = null;
        }
    }
}
