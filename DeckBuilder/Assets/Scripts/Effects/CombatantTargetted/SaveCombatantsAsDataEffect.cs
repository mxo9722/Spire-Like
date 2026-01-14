using System.Collections.Generic;
using UnityEngine;

public class SaveCombatantsAsDataEffect : CombatantTargetEffect
{
    [SerializeField] private string _dataKey = "TargetUnits";
    [SerializeField] private SaveDataLevel _saveDataLevel = SaveDataLevel.CONTEXT;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        return new SaveDataGA(context, _dataKey, combatantTargets, _saveDataLevel);
    }
}
