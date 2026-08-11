using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class SetStatusEffectEffect : CombatantTargetEffect, IDynamicEffectText
{
    [SerializeField] private StatusEffect _statusEffectType;
    [SerializeReference, SR] private Quantity _stackCount = new SetQ();
    [SerializeField] private bool _skipAnimation;

    public StatusEffect StatusEffectType { get => _statusEffectType; }
    public Quantity StackCount { get => _stackCount; }
    public StatusEffectInfo SEInfo => StatusEffectSystem.GetDictionaryEntry(_statusEffectType);

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        return new SetStatusEffectGA(SEInfo, _stackCount.GetAmount(context), combatantTargets, context, _skipAnimation);
    }

    public string GetStaticText()
    {
        return _stackCount.GetStaticAmount().ToString();
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        if (context.Caster == null && context.PlayedCard != null)
        {
            context = context.Clone();
            context.SetCaster(context.PlayedCard.GetOwnerView(context));
        }

        return StatusEffectSystem.StackAdditionValueFromEffect(SEInfo, _stackCount.GetAmount(context), context, targetCombatants);
    }

    public override List<StatusEffect> GetAllStatusEffects()
    {
        return new() { _statusEffectType };
    }
}
