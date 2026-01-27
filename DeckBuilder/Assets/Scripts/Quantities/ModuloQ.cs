using SerializeReferenceEditor;
using UnityEngine;

public class ModuloQ : Quantity
{

    [SerializeReference, SR] private Quantity _a = new SetQ();
    [SerializeReference, SR] private Quantity _n = new SetQ();

    public override int GetAmount(EffectContext effectContext)
    {
        return _a.GetAmount(effectContext) % _n.GetAmount(effectContext);
    }

    public override int GetStaticAmount()
    {
        return _a.GetStaticAmount() % _n.GetStaticAmount();

    }
}
