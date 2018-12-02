using System.Collections;
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
        _characterAvatarPosX = (_characterAvatarHeight / 2) + _characterAvatarMargin;
        _initialCharacterAvatarPosY = _characterAvatarPosX * -1;
    }

	// Use this for initialization
	public void UpdateUI () {
        int posY = _initialCharacterAvatarPosY;

        foreach (var Player in PlayerController.Players) {
            Vector3 position = new Vector3(_characterAvatarPosX, posY, 0);

            var characterAvatar = Instantiate(CharacterAvatarPrefab, Vector3.zero, Quaternion.identity, Content.transform);

            Debug.Log(Player.Abilities);

            characterAvatar.gameObject.SetActive(true);
            characterAvatar.Character = Player;

            characterAvatar.transform.localPosition = position;

            var ImageComponent = characterAvatar.Image.GetComponent<Image>();
            ImageComponent.sprite = Player.CharacterInfo.avatar;
            TextMeshProUGUI avatarName = characterAvatar.AvatarName.GetComponent<TextMeshProUGUI>();
            avatarName.SetText(Player.CharacterInfo.name);

            characterAvatar.AvatarClicked += OnAvatarClick;
            characterAvatar.CharacterAbilityClicked += OnAbilityClick;

            characterAvatar.CreateActions();

            posY = posY - (_characterAvatarHeight + _characterAvatarMargin);
        }
    }

	// Update is called once per frame
	private void Update () {

	}

    public void OnAvatarClick (CharacterAvatar CharacterAvatar) {
        CharacterAvatar.SetSelected(true);

        if (_activeCharacterAvatar != null) {
            if (CharacterAvatar == _activeCharacterAvatar) {
                return;
            }

            Debug.Log(_activeCharacterAvatar);
            _activeCharacterAvatar.SetSelected(false);
        }

        if (_activeCharacterAbility != null) {
            _activeCharacterAbility.SetSelected(false);
            _activeCharacterAbility = null;
        }


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
        }

        if (_activeCharacterAvatar != null && _activeCharacterAbility != null) {
            _activeCharacterAvatar.SetSelected(false);
            _activeCharacterAvatar = null;
        }

//         if (_activeCharacterAvatar == null) {
//             if (_activeCharacterAbility != null) {
//                 GameController.Instance.SelectCharacter(characterAbility.CharacterAvatar.Character);
//             } else {
//                 GameController.Instance.SelectCharacter(null);
//             }
//         } else if (characterAbility.CharacterAvatar )
//
//         characterAbility.SetSelected(_isAbilityActive);
// ;
//         GameController.Instance.SetAbilityActive(_isAbilityActive);


    }
}
