using SerializeReferenceEditor;
using System;
using UnityEngine;

public class PowerOfQ : Quantity
{
    [SerializeReference, SR] private Quantity _base = new SetQ(0);
    [SerializeReference, SR] private Quantity _exponent = new SetQ(0);


    public override int GetAmount(EffectContext effectContext)
    {
        int baseValue = _base.GetAmount(effectContext);
        int exponentValue = _exponent.GetAmount(effectContext);

        return (int)Math.Pow(baseValue, exponentValue);
    }

    public override int GetStaticAmount()
    {
        int baseValue = _base.GetStaticAmount();
        int exponentValue = _exponent.GetStaticAmount();

        return (int)Math.Pow(baseValue, exponentValue);
    }
}
