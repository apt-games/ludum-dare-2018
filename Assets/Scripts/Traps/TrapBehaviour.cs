using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    private TrapEffectBehaviour _effect;
    public TrapType TrapType { get; private set; }

    public int Uses = 1;

    public void TrapTriggered(CharacterBehaviour character = null)
    {
        if (Uses > 0)
        {
            // kill potential character
            if (character != null)
                GameController.Instance.PlayerController.KillCharacter(character);

            // show effect
            _effect.TriggerEffectEvent.Invoke();

            //decrement uses
            Uses --;
        }
    }

    public void Trap()
    {
        if (Uses > 0)
        {
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
