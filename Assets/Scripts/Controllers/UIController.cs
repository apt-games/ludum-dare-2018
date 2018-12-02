using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    private int _characterAvatarHeight = 128;
    private int _characterAvatarMargin = 20;
    private int _characterAvatarPosX {get; set;}
    private int _initialCharacterAvatarPosY {get; set;}

    private CharacterAvatar ActiveCharacterAvatar {get; set;}
    public GameObject Content;
    public PlayerController PlayerController;
    public CharacterAvatar CharacterAvatarPrefab;
    private readonly List<CharacterAvatar> _characterAvatars = new List<CharacterAvatar>();

    public UIController() {
        _characterAvatarPosX = (_characterAvatarHeight / 2) + _characterAvatarMargin;
        _initialCharacterAvatarPosY = _characterAvatarPosX * -1;
    }

	// Use this for initialization
	private void Start () {
        int posY = _initialCharacterAvatarPosY;

        foreach (var Player in PlayerController.Players) {
            Vector3 position = new Vector3(_characterAvatarPosX, posY, 0);
            var characterAvatar = Instantiate(CharacterAvatarPrefab, Vector3.zero, Quaternion.identity, Content.transform);
            characterAvatar.transform.localPosition = position;

            characterAvatar.AvatarClicked += OnAvatarClick;

            posY = posY - (_characterAvatarHeight + _characterAvatarMargin);
        }
    }

	// Update is called once per frame
	private void Update () {

	}

    public void OnAvatarClick (CharacterAvatar CharacterAvatar) {

        if (ActiveCharacterAvatar != null) {
            if (CharacterAvatar == ActiveCharacterAvatar) {
                return;
            }

            Debug.Log(ActiveCharacterAvatar);
            ActiveCharacterAvatar.SetSelected(false);
        }

        ActiveCharacterAvatar = CharacterAvatar;
        CharacterAvatar.SetSelected(true);
    }
}
