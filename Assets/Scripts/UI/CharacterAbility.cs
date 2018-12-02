using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Animator))]
public class CharacterAbility : MonoBehaviour {

    private Animator _animator;
    [HideInInspector]
    public AbilityBehaviour Ability;
    [HideInInspector]
    public CharacterAvatar CharacterAvatar;
    public Image Image;

    public event Action<CharacterAbility> AbilityClicked;

    public void OnAbilityClick () {
        AbilityClicked?.Invoke(this);
    }

    public void SetSelected(bool selected) {
        if (selected) {
            _animator.SetTrigger("setabilityselected");
        } else {
            _animator.SetTrigger("setabilityunselected");
        }
    }

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
