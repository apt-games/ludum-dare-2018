using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private bool _isWalking;

    public bool IsAlive = true;

    private void Awake ()
    {
        _animator = GetComponentInChildren<Animator>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
    }

    public void MoveTo(Transform room)
    {
        Debug.Log("Moving character to " + room.position);

        var target = new Vector3(room.position.x, room.position.y, transform.position.z);

        _agent.SetDestination(target);
        SetWalking(true);
    }

    public void Die()
    {
        Debug.Log("Died");
        IsAlive = false;
        _animator.SetTrigger("die");
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
