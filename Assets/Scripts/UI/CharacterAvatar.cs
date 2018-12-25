using System;
﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Animator))]
public class CharacterAvatar : MonoBehaviour {
    private int _characterAbilityHeight = 96;
    private int _characterAbilityMargin = 20;
    private int _initialCharacterAbilityPosX {get; set;}
    private int _characterAbilityPosY {get; set;}
    private Dictionary<AbilityType, Texture2D> _abilityTextures = new Dictionary<AbilityType, Texture2D>();
    private Dictionary<AbilityType, Sprite> _abilityIcons = new Dictionary<AbilityType, Sprite>();

    [HideInInspector]
    public Animator _animator;

    public event Action<CharacterAvatar> AvatarClicked;
    public event Action<CharacterAbility> CharacterAbilityClicked;
    public GameObject Actions;
    public GameObject Image;
    public GameObject AvatarName;
    public GameObject SpeechBubble;
    [HideInInspector]
    public CharacterBehaviour Character;
    public CharacterAbility CharacterAbilityPrefab;
    public GameObject Selected;

    public CharacterAvatar() {
        _characterAbilityPosY = 0;
        // _characterAbilityPosY = ((_characterAbilityHeight / 2) + _characterAbilityMargin) * -1;
        _initialCharacterAbilityPosX = _characterAbilityPosY * -1;
    }

    private void Awake() {
        _animator = GetComponent<Animator>();

        Sprite clairvoyanceIcon = Resources.Load<Sprite>("UI/clairvoyance");
        _abilityIcons.Add(AbilityType.Clairvoyance, clairvoyanceIcon );
        Texture2D clairvoyanceIconTexture = Resources.Load<Texture2D>("UI/cursors/clairvoyance");
        _abilityTextures.Add(AbilityType.Clairvoyance, clairvoyanceIconTexture );

        Sprite flareIcon = Resources.Load<Sprite>("UI/flare");
        _abilityIcons.Add(AbilityType.Flare, flareIcon );
        Texture2D flareIconTexture = Resources.Load<Texture2D>("UI/cursors/flare");
        _abilityTextures.Add(AbilityType.Flare, flareIconTexture );

        Sprite stoneIcon = Resources.Load<Sprite>("UI/stone");
        _abilityIcons.Add(AbilityType.Stone, stoneIcon );
        Texture2D stoneIconTexture = Resources.Load<Texture2D>("UI/cursors/stone");
        _abilityTextures.Add(AbilityType.Stone, stoneIconTexture );

        Sprite reviveIcon = Resources.Load<Sprite>("UI/medikit");
        _abilityIcons.Add(AbilityType.Revive, reviveIcon );
        Texture2D reviveIconTexture = Resources.Load<Texture2D>("UI/cursors/medikit");
        _abilityTextures.Add(AbilityType.Revive, reviveIconTexture );
    }

    public void OnAvatarClick () {
        AvatarClicked?.Invoke(this);
    }

    public void OnAbilityClick (CharacterAbility characterAbility) {
        CharacterAbilityClicked?.Invoke(characterAbility);
    }

    public void SetSelected(bool selected) {
        CanvasGroup SelectedCanvasGroup = Selected.GetComponent<CanvasGroup>();

        if (selected) {
            // _animator.SetTrigger("setavatarselected");
            SelectedCanvasGroup.alpha = 1f;
        } else {
            // _animator.SetTrigger("setunavatarselected");
            SelectedCanvasGroup.alpha = 0f;
        }
    }

    public void CreateActions() {
        int posX = _initialCharacterAbilityPosX;

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
                characterAbility.texture = _abilityTextures[type];

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

    public void ShowDialogue(DialogueLine dialogue, float fadeTime)
    {
        StartCoroutine(ShowAndFadeDialogue(dialogue, fadeTime));
    }

    IEnumerator ShowAndFadeDialogue(DialogueLine dialogue, float fadeTime)
    {
        CanvasGroup dialogueCanvasGroup = SpeechBubble.GetComponent<CanvasGroup>();
        TextMeshProUGUI uiText = SpeechBubble.GetComponent<TextMeshProUGUI>();

        dialogueCanvasGroup.alpha = 1;
        uiText.text = dialogue.Text;

        yield return new WaitForSeconds(dialogue.Duration);

        const float FADE_STEPS = 30.0f;
        const float stepsize = 1.0f / FADE_STEPS;
        float waitTime = fadeTime / FADE_STEPS;
        for(float f = 1; f >= 0; f -= stepsize)
        {
            dialogueCanvasGroup.alpha = f;
            yield return new WaitForSeconds(waitTime);
        }
        dialogueCanvasGroup.alpha = 0;
    }
}
