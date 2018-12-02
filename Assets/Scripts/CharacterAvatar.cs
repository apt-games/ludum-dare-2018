using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class CharacterAvatar : MonoBehaviour {
    public event Action<CharacterAvatar> AvatarClicked;
    private Animator _animator;
    public GameObject actions;

    public void OnMouseEnter () {
        _animator.SetTrigger("showactions");
    }

    public void OnMouseExit () {
        _animator.SetTrigger("hideactions");
    }

    private void Awake() {
        _animator = GetComponent<Animator>();
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

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
