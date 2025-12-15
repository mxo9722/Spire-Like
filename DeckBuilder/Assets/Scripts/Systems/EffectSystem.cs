using System.Collections;
using UnityEngine;

public class EffectSystem : MonoBehaviour
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
        GameAction effectAction = performEffectsGA.Effect.GetGameAction(HeroSystem.Instance.HeroView, combatantTargets: performEffectsGA.CombatantTargets, laneTargets:performEffectsGA.LaneTargets, cardTargets: performEffectsGA.CardTargets);
        if (effectAction != null)
            ActionSystem.Instance.AddReaction(effectAction);
        else
            Debug.LogError("Null GameAction attempted to be applied!");
        yield return null;
    }
}
