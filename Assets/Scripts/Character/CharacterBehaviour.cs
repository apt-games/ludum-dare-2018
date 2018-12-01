using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake ()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Transform room)
    {
        Debug.Log("Moving character to " + room.position);

        var target = new Vector3(room.position.x, room.position.y, transform.position.z);

        _agent.SetDestination(target);
    }
}
