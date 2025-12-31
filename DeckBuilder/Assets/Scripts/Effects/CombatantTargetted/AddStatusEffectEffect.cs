using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffectEffect : CombatantTargetEffect, IDynamicEffectText
{
    [SerializeField] private StatusEffectType _statusEffectType;
    [SerializeReference, SR] private Quantity _stackCount = new SetQ();
    [SerializeField] private bool _skipAnimation;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        return new AddStatusEffectGA(_statusEffectType, _stackCount.GetAmount(context), combatantTargets, context.Caster, _skipAnimation);
    }

    public string GetStaticText()
    {
        return _stackCount.GetStaticAmount().ToString();
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return StatusEffectSystem.StackAdditionValueFromEffect(_statusEffectType, _stackCount.GetAmount(context), context.Caster, targetCombatants);
    }

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        return new() { _statusEffectType };
    }
}
