using SerializeReferenceEditor;
using UnityEngine;

public class AddQ : Quantity
{

    [SerializeReference, SR] private Quantity _a = new SetQ();
    [SerializeReference, SR] private Quantity _b = new SetQ();

    public override int GetAmount(EffectContext effectContext)
    {
        return _a.GetAmount(effectContext) + _b.GetAmount(effectContext);
    }

    public override int GetStaticAmount()
    {
        return _a.GetStaticAmount() + _b.GetStaticAmount();
    }
}
