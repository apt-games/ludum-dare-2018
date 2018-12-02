using UnityEngine;
using UnityEngine.Events;

public class TriggerBehaviour : MonoBehaviour
{
    public TriggerEvent OnTriggerEntered;

    private void OnTriggerEnter(Collider collider)
    {
        var character = collider.gameObject.GetComponent<CharacterBehaviour>();

        if (character != null && !character.IsAlive)
        {
            // check if character is dead, don't count it for triggering
            return;
        }

        Debug.Log("TRIGGERED " + transform.position);
        OnTriggerEntered?.Invoke(character);
    }

    [System.Serializable]
    public class TriggerEvent : UnityEvent<CharacterBehaviour>
    {
    }
}

