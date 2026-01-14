using System;
using System.Collections;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem>
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<MultipleEffectsGA>(MultipleEffectsPerformer);
        ActionSystem.AttachPerformer<PerformEffectsGA>(PerformEffectPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<MultipleEffectsGA>();
        ActionSystem.DetachPerformer<PerformEffectsGA>();
    }

    private IEnumerator MultipleEffectsPerformer(MultipleEffectsGA arg)
    {
        foreach(AutoTargetEffect effect in arg.Effects)
            ActionSystem.Instance.AddReaction(effect.GetGameAction(arg.Context));

        yield return null;
    }

    private IEnumerator PerformEffectPerformer(PerformEffectsGA performEffectsGA)
    {
        GameAction effectAction = performEffectsGA.Effect.GetGameAction(performEffectsGA.Context, combatantTargets: performEffectsGA.CombatantTargets, laneTargets:performEffectsGA.LaneTargets, cardTargets: performEffectsGA.CardTargets);
        if (effectAction != null)
            ActionSystem.Instance.AddReaction(effectAction);

        yield return null;
    }
}
