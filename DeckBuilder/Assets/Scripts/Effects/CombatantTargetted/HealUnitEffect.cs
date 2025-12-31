using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class HealUnitEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private Quantity _amount = new SetQ();

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        HealUnitsGA healUnitsGA = new(combatantTargets, _amount.GetAmount(context));

        return healUnitsGA;
    }
}
