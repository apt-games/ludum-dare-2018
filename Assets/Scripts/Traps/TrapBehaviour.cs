using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    private TrapEffectBehaviour _effect;
    public TrapType TrapType { get; private set; }

    public void TrapTriggered(CharacterBehaviour character)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerController
            .KillCharacter(character);

        _effect.TriggerEffectEvent.Invoke();
    }

    public void SetEffect(TrapEffectBehaviour effect)
    {
        effect.transform.SetParent(transform );
        TrapType = effect.Type;
        _effect = effect;
    }
}

public enum TrapType
{
    Flamethrower,
    Sawblade,
    Gas,
    Electric
}
