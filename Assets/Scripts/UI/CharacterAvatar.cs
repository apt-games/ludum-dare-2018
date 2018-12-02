using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Animator))]
public class CharacterAvatar : MonoBehaviour {
    private int _characterAbilityHeight = 32;
    private int _characterAbilityMargin = 10;
    private int _initialCharacterAbilityPosX {get; set;}
    private int _characterAbilityPosY {get; set;}
    private Dictionary<AbilityType, Sprite> _abilityIcons = new Dictionary<AbilityType, Sprite>();

    private Animator _animator;

    public event Action<CharacterAvatar> AvatarClicked;
    public GameObject Actions;
    public GameObject Image;
    public GameObject AvatarName;
    [HideInInspector]
    public CharacterBehaviour Character;
    public CharacterAbility CharacterAbilityPrefab;

    public CharacterAvatar() {
        _characterAbilityPosY = ((_characterAbilityHeight / 2) + _characterAbilityMargin) * -1;
        _initialCharacterAbilityPosX = _characterAbilityPosY * -1;
    }

    public void OnMouseEnter () {
        _animator.SetTrigger("showactions");
    }

    public void OnMouseExit () {
        _animator.SetTrigger("hideactions");
    }

    private void Awake() {
        // _animator = GetComponent<Animator>();

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
            Debug.Log(Ability.Ability.Type);
            var type = Ability.Ability.Type;

            Vector3 position = new Vector3(posX, _characterAbilityPosY, 0);

            var characterAbility = Instantiate(CharacterAbilityPrefab, Vector3.zero, Quaternion.identity, Actions.transform);

            characterAbility.Ability = Ability;

            characterAbility.transform.localPosition = position;

            Debug.Log("Hei");

            var ImageComponent = characterAbility.GetComponent<Image>();
            Debug.Log("Hei");
            Debug.Log(ImageComponent);
            ImageComponent.sprite = _abilityIcons[type];

            // TODO: Add click handler for abilities
            // characterAbility.AvatarClicked += OnAvatarClick;

            posX = posX + _characterAbilityHeight + _characterAbilityMargin;


        }
    }

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}
}
