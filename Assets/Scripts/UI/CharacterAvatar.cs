using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Animator))]
public class CharacterAvatar : MonoBehaviour {
    private int _characterAbilityHeight = 48;
    private int _characterAbilityMargin = 20;
    private int _initialCharacterAbilityPosX {get; set;}
    private int _characterAbilityPosY {get; set;}
    private Dictionary<AbilityType, Sprite> _abilityIcons = new Dictionary<AbilityType, Sprite>();

    private Animator _animator;

    public event Action<CharacterAvatar> AvatarClicked;
    public event Action<CharacterAbility> CharacterAbilityClicked;
    public GameObject Actions;
    public GameObject Image;
    public GameObject AvatarName;
    [HideInInspector]
    public CharacterBehaviour Character;
    public CharacterAbility CharacterAbilityPrefab;

    public CharacterAvatar() {
        _characterAbilityPosY = 0;
        // _characterAbilityPosY = ((_characterAbilityHeight / 2) + _characterAbilityMargin) * -1;
        _initialCharacterAbilityPosX = _characterAbilityPosY * -1;
    }

    private void Awake() {
        _animator = GetComponent<Animator>();

        Sprite clairvoyanceIcon = Resources.Load<Sprite>("UI/clairvoyance");
        _abilityIcons.Add(AbilityType.Clairvoyance, clairvoyanceIcon );

        Sprite flareIcon = Resources.Load<Sprite>("UI/flare");
        _abilityIcons.Add(AbilityType.Flare, flareIcon );

        Sprite stoneIcon = Resources.Load<Sprite>("UI/stone");
        _abilityIcons.Add(AbilityType.Stone, stoneIcon );

        Sprite reviveIcon = Resources.Load<Sprite>("UI/medikit");
        _abilityIcons.Add(AbilityType.Revive, reviveIcon );
    }

    public void OnAvatarClick () {
        AvatarClicked?.Invoke(this);
    }

    public void OnAbilityClick (CharacterAbility characterAbility) {
        CharacterAbilityClicked?.Invoke(characterAbility);
    }

    public void SetSelected(bool selected) {
        if (selected) {
            _animator.SetTrigger("setavatarselected");
        } else {
            _animator.SetTrigger("setunavatarselected");
        }
    }

    public void CreateActions() {
        int posX = _initialCharacterAbilityPosX;


        Debug.Log(Character.Abilities.Count);

        foreach (var Ability in Character.Abilities) {
            var type = Ability.Ability.Type;

            for (int i = 0; i < Ability.Uses; i++) {
                Vector3 position = new Vector3(posX, _characterAbilityPosY, 0);

                var characterAbility = Instantiate(CharacterAbilityPrefab, Vector3.zero, Quaternion.identity, Actions.transform);

                characterAbility.gameObject.SetActive(true);

                characterAbility.CharacterAvatar = this;
                characterAbility.Ability = Ability;

                characterAbility.transform.localPosition = position;

                characterAbility.Image.sprite = _abilityIcons[type];

                characterAbility.AbilityClicked += OnAbilityClick;

                posX = posX + _characterAbilityHeight + _characterAbilityMargin;
            }


        }
    }

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}
}
