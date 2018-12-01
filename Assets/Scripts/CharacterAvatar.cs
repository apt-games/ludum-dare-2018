using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class CharacterAvatar : MonoBehaviour {

    private Animator _animator;
    public GameObject actions;

    public void OnMouseEnter () {
        Debug.Log("Enter!");

        _animator.SetTrigger("showactions");
    }

    public void OnMouseExit () {
        Debug.Log("Exit!");

        _animator.SetTrigger("hideactions");
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
