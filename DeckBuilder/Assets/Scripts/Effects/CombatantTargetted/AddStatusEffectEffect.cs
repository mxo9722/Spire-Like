using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffectEffect : CombatantTargetEffect, IDynamicEffectText
{
    [SerializeField] private StatusEffectType _statusEffectType;
    [SerializeField] private int _stackCount;

    protected override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets)
    {
        return new AddStatusEffectGA(_statusEffectType, _stackCount, combatantTargets, caster);
    }

    public string GetStaticText()
    {
        return _stackCount.ToString();
    }

    public string GetDynamicText(CombatantView caster, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return StatusEffectSystem.StackAdditionValueFromEffect(_statusEffectType, _stackCount, caster, targetCombatants);
    }

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        return new() { _statusEffectType };
    }
}
