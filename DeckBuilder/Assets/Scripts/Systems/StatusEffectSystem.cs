using System.Collections;
using UnityEngine;

public class StatusEffectSystem : Singleton<StatusEffectSystem>
{

    [SerializeField] private GameObject _defendVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        float waitTime = 0;

        foreach (CombatantView target in addStatusEffectGA.Targets)
        {
            switch (addStatusEffectGA.StatusEffectType)
            {
                case StatusEffectType.ARMOR:
                    GameObject effect = Instantiate(_defendVFX, target.transform);
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localScale = Vector3.one;
                    waitTime = 1.5f;
                    break;
                default:
                    break;
            }
        }

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);

        foreach (CombatantView target in addStatusEffectGA.Targets)
        {
            target.AddStatusEffect(addStatusEffectGA.StatusEffectType, addStatusEffectGA.StackCount);
            ///TODO: Add a special effect

            yield return null;
        }

    }
}
