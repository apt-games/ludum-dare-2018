using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CapsuleCollider))]
public class CharacterBehaviour : MonoBehaviour
{
    public int ID;

    public AudioClip[] AllFootSteps;
    public AudioSource FootStepsAudioSource;

    public Light Light;

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
    public bool CanWalk = false;

    public bool IsWalking { get; private set; }
    public bool IsAlive { get; private set; } = true;

    private CharacterInfo _characterInfo;

    private void Awake ()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CapsuleCollider>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;

        int footStepAudioIndex = Random.Range(0, AllFootSteps.Length);
        var _footStepsAudio = AllFootSteps[footStepAudioIndex];
        FootStepsAudioSource.clip = _footStepsAudio;

        if (!PlayerController.HasWokenFirstCharacter) {
            _animator.SetTrigger("dead");
            PlayerController.HasWokenFirstCharacter = true;

            StartCoroutine(WakeCharacter());
        } else {
            CanWalk = true;
        }
    }

    private IEnumerator WakeCharacter() {
        yield return new WaitForSeconds(5f);
        _animator.SetTrigger("wake");
        yield return new WaitForSeconds(0.5f);
        CanWalk = true;
    }

    private void Update()
    {
        if (IsWalking)
        {
            FootStepsAudioSource.loop = true;
            float dist = _agent.remainingDistance;
            if (dist != Mathf.Infinity && _agent.pathStatus == NavMeshPathStatus.PathComplete &&
                _agent.remainingDistance < _agent.stoppingDistance)
            {
                // reached target
                SetWalking(false);
            }
        }
    }

    public void TeleportToWorldPositions(Vector3 position)
    {
        _agent.enabled = false;
        transform.position = position;
        _agent.enabled = true;
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

        const float randomRadius = 0.1f;
        var target = new Vector3(p.x + Random.Range(-randomRadius, randomRadius), p.y + Random.Range(-randomRadius, randomRadius), transform.position.z);

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
        Light.intensity = 0;
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

        if (walking) {
            FootStepsAudioSource.Play();
        } else {
            FootStepsAudioSource.Stop();
        }
    }
}
