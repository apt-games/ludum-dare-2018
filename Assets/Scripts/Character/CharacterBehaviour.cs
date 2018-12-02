using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CapsuleCollider))]
public class CharacterBehaviour : MonoBehaviour
{
    public int ID;

    public RoomBehaviour OccupyingRoom { get; set; }

    public CharacterInfo CharacterInfo
    {
        get { return _characterInfo; }
        set
        {
            if (_characterInfo != value)
            {
                _characterInfo = value;
                SetColors(_characterInfo.colors);
            }
        }
    }

    public List<AbilityBehaviour> Abilities { get; } = new List<AbilityBehaviour>();

    public CharacterBody CharacterBody;

    private NavMeshAgent _agent;
    private Animator _animator;
    private CapsuleCollider _collider;

    public bool IsWalking { get; private set; }
    public bool IsAlive { get; private set; } = true;

    private CharacterInfo _characterInfo;

    private void Awake ()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CapsuleCollider>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
    }

    private void Update()
    {
        if (IsWalking)
        {
            float dist = _agent.remainingDistance;
            if (dist != Mathf.Infinity && _agent.pathStatus == NavMeshPathStatus.PathComplete &&
                _agent.remainingDistance < _agent.stoppingDistance)
            {
                // reached target
                SetWalking(false);
            }
        }
    }

    public void SetColors(CharacterColors colors)
    {
        CharacterBody.Body.color = colors.Body;
        CharacterBody.Head.color = colors.Head;
        foreach (var leg in CharacterBody.Legs)
            leg.color = colors.Legs;
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
        IsWalking = false;
        IsAlive = false;
        _animator.SetTrigger("die");
        _agent.isStopped = true;
        _collider.enabled = false;
    }

    public void SetDead(bool dead)
    {
        IsAlive = !dead;
        if (dead)
        {
            _animator.SetTrigger("die");
            _collider.enabled = false;
        }
    }

    public void AddAbility(AbilityBehaviour ability)
    {
        ability.transform.SetParent(transform, false);
        ability.transform.localPosition = Vector3.zero;
        Abilities.Add(ability);
    }

    private void SetWalking(bool walking)
    {
        IsWalking = walking;
        _animator.SetBool("walking", IsWalking);
    }
}
