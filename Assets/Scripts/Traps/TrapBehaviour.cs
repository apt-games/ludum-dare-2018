using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    public TrapType TrapType { get; }

    public void TrapTriggered(CharacterBehaviour character)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerController
            .KillCharacter(character);
    }
}

public enum TrapType
{
    Flamethrower,
    Sawblade,
    Gas,
    Electric
}
