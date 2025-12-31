using System.Collections;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem>
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectsGA>(PerformEffectPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectsGA>();
    }

    private IEnumerator PerformEffectPerformer(PerformEffectsGA performEffectsGA)
    {
        GameAction effectAction = performEffectsGA.Effect.GetGameAction(performEffectsGA.Context, combatantTargets: performEffectsGA.CombatantTargets, laneTargets:performEffectsGA.LaneTargets, cardTargets: performEffectsGA.CardTargets);
        if (effectAction != null)
            ActionSystem.Instance.AddReaction(effectAction);

        yield return null;
    }
}
