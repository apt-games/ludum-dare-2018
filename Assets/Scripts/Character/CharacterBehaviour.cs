﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private bool _isWalking;

    public bool IsAlive = true;

    public RoomBehaviour OccupyingRoom;

    public CharacterInfo CharacterInfo;

    public List<AbilityBehaviour> Abilities { get; } = new List<AbilityBehaviour>();

    private void Awake ()
    {
        _animator = GetComponentInChildren<Animator>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
    }

    private void Start()
    {
        Abilities.AddRange(GetComponentsInChildren<AbilityBehaviour>());
    }

    public void MoveTo(RoomBehaviour room)
    {
        var p = room.transform.position;
        OccupyingRoom = room;

        Debug.Log("Moving character to " + p);

        var target = new Vector3(p.x, p.y, transform.position.z);

        _agent.SetDestination(target);
        SetWalking(true);
    }

    public void UseAbility()
    {
        var ability = GetComponentInChildren<AbilityBehaviour>();
        if (ability != null)
            ability.UseAbility();
    }

    public void Die()
    {
        Debug.Log("Died");
        IsAlive = false;
        _animator.SetTrigger("die");
        _agent.isStopped = true;
    }

    private void SetWalking(bool walking)
    {
        _isWalking = walking;
        _animator.SetBool("walking", _isWalking);
    }

    private void Update()
    {
        if (_isWalking && !_agent.hasPath)
        {
            SetWalking(false);
        }
    }
}
