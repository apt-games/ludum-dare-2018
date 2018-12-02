using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    private TrapEffectBehaviour _effect;
    public TrapType TrapType { get; private set; }

    public int Uses = 1;

    public void TrapTriggered(CharacterBehaviour character)
    {
        if (Uses > 0)
        {
            GameController.Instance.PlayerController.KillCharacter(character);
            _effect.TriggerEffectEvent.Invoke();
            Uses--;
        }
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
