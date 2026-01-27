using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeAnotherUnitAttackFoeEffect : CombatantTargetEffect, IDynamicEffectText
{

    [SerializeReference, SR] private CombatantTargetMode _targetMode;
    [SerializeField] private AttackHeroEffect _effect;


    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        CombatantView caster = combatantTargets.First();

        if (caster == null)
            return null;

        EffectContext newContext = new(context);
        newContext.SetCaster(caster);

        List<CombatantView> targets = _targetMode.GetTargets(newContext);

        GameAction gameAction = _effect.GetGameAction(newContext, targets);

        return gameAction;
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        CombatantView caster = targetCombatants.First();

        if (caster == null)
            return GetStaticText();

        EffectContext newContext = new(context);
        newContext.SetCaster(caster);

        List<CombatantView> targets = _targetMode.GetTargets(newContext);

        return _effect.GetDynamicText(newContext, targets);
    }

    public string GetStaticText()
    {
        return _effect.GetStaticText();
    }
}
