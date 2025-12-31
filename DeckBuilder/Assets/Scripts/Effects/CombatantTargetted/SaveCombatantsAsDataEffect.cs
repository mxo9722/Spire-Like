using System.Collections.Generic;
using UnityEngine;

public class SaveCombatantsAsDataEffect : CombatantTargetEffect
{
    [SerializeField] private string _dataKey = "TargetUnits";

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        return new SaveDataGA(context, _dataKey, combatantTargets);
    }
}
