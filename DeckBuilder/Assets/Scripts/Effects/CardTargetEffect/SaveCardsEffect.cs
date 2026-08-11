using System.Collections.Generic;
using UnityEngine;

public class SaveCardsEffect : CardTargetEffect
{
    [SerializeField] private string _dataKey = "TargetCards";
    [SerializeField] private SaveDataLevel _saveDataLevel = SaveDataLevel.CONTEXT;

    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        return new SaveDataGA(context, _dataKey, cardTargets, _saveDataLevel);
    }
}
