using SerializeReferenceEditor;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class AutoTargetlessEffect : AutoTargetEffect
{
    public override Effect[] Effects { get => new Effect[] { _noTargetEffect }; }
    [field: SerializeReference, SR] private NoTargetEffect _noTargetEffect;

    public override GameAction GetGameAction(EffectContext context)
    {
        PerformEffectsGA performEffectsGA = new(context, _noTargetEffect);
        return performEffectsGA;
    }

    public override bool RequiresUserInput()
    {
        return _noTargetEffect is INeedsUserInput;
    }

    public override IEnumerator WaitForUserInput(EffectContext context)
    {
        if(_noTargetEffect is INeedsUserInput needsUserInput)
        {
            yield return needsUserInput.WaitForUserInput(context);
        }
        else
            yield return null;
    }

    public override AutoTargetEffect[] GetNestedEffects()
    {
        return _noTargetEffect.GetNestedEffects();
    }

    public override void SimulatedPerform(EffectContext context)
    {
        if (GetGameAction(context) is SimulatedGameAction simulatedGameAction)
            simulatedGameAction.SimulatedPerform(context);
    }
}
