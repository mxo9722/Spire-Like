using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class SaveQuantityToUnitEffect : CombatantTargetEffect
{

    [SerializeField] private string _key;
    [SerializeReference, SR] private Quantity _value;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        int value = _value.GetAmount(context);

        SaveDataToUnitsGA saveDataToUnitsGA = new(combatantTargets, _key, value);
        return saveDataToUnitsGA;
    }
}
