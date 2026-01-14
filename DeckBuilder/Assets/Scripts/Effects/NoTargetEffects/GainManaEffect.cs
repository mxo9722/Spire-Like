using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class GainManaEffect : NoTargetEffect
{
    [SerializeReference, SR] private Quantity _amount = new SetQ();

    protected override GameAction GetGameAction(EffectContext context)
    {
        GainManaGA gainManaGA = new(_amount.GetAmount(context));
        return gainManaGA;
    }
}
