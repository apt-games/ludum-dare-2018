using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;

    public Animator Animator;

    private bool _isWalking;

    private void Awake ()
    {
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

    private void SetWalking(bool walking)
    {
        _isWalking = walking;
        Animator.SetBool("walking", _isWalking);
    }

    private void Update()
    {
        if (_isWalking && !_agent.hasPath)
        {
            SetWalking(false);
        }
    }
}
