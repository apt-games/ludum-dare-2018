using System.Collections;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    private TrapEffectBehaviour _effect;
    public TrapType TrapType { get; private set; }

    public void TrapTriggered(CharacterBehaviour character = null)
    {
        if (_effect.Uses > 0)
        {
            // kill potential character
            if (character != null)
            {
                StartCoroutine(KillCharacterAfterDelay(character));
            }

            // show effect
            _effect.TriggerEffectEvent.Invoke();

            //decrement uses
            _effect.Uses--;
        }
    }

    private IEnumerator KillCharacterAfterDelay(CharacterBehaviour character)
    {
        yield return new WaitForSeconds(_effect.Delay);
        GameController.Instance.PlayerController.KillCharacter(character);
    }

    public void Trap()
    {
        if (_effect.Uses > 0)
        {
            _effect.TriggerEffectEvent.Invoke();
            _effect.Uses--;
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
