using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StatusEffectReaction
{
    [SerializeField] protected ReactionTiming _reactionTiming;
    [field: SerializeReference, SR] public List<AutoTargetEffect> Effects { get; private set; } = new();

    protected StatusEffectType _type;

    public void SetUp(StatusEffectType type)
    {
        _type = type;
    }

    public virtual void InvokeEffects(CombatantView owner)
    {
        EffectContext context = new EffectContext(owner);

        foreach (AutoTargetEffect effect in Effects)
        {
            GameAction gameAction = effect.GetGameAction(context);

            if (gameAction is PerformEffectsGA performEffectGA)
            {
                GameAction effectAcion = performEffectGA.Effect.GetGameAction(new(performEffectGA.Caster), performEffectGA.CombatantTargets, performEffectGA.LaneTargets, performEffectGA.CardTargets);
                
                if(effectAcion != null)
                    ActionSystem.Instance.AddReaction(effectAcion);
            }
            else
                ActionSystem.Instance.AddReaction(gameAction);
        }
    }

    public abstract void SubscribeCondition(object subscriber, Action<GameAction> reaction);
    public abstract void UnsubscribeCondition(object subscriber, Action<GameAction> reaction);
    public abstract int SubConditionIsMet(CombatantView owner, GameAction gameAction);
}
