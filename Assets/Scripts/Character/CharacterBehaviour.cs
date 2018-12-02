using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBehaviour : MonoBehaviour
{
    public RoomBehaviour OccupyingRoom;
    public CharacterInfo CharacterInfo;
    public List<AbilityBehaviour> Abilities { get; } = new List<AbilityBehaviour>();

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isWalking;
    public bool IsAlive = true;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

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

    private void Update()
    {
        if (_isWalking && !_agent.hasPath)
        {
            SetWalking(false);
        }
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

    public void AddAbility(AbilityBehaviour ability)
    {
        ability.transform.SetParent(transform, false);
        ability.transform.localPosition = Vector3.zero;
        Abilities.Add(ability);
    }

    private void SetWalking(bool walking)
    {
        _isWalking = walking;
        _animator.SetBool("walking", _isWalking);
    }
}
