using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AddCustomSEEffect : CombatantTargetEffect, IDynamicEffectText
{
    [SerializeReference, SR] private SEIdentifier _statusEffect;
    [SerializeReference, SR] private Quantity _stackCount = new SetQ();
    [SerializeField] private bool _skipAnimation;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        AddStatusEffectGA addStatusEffectGA = new(_statusEffect.GetSEInfo(), _stackCount.GetAmount(context), combatantTargets, context, _skipAnimation);
        return addStatusEffectGA;
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return _stackCount.GetAmount(context).ToString();
    }

    public string GetStaticText()
    {
        return _stackCount.GetStaticAmount().ToString();
    }
}
