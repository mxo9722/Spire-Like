using SerializeReferenceEditor;
using UnityEngine;

public class ReduceModifier : Modifier
{
    [SerializeReference, SR] private Quantity _amount = new SetQ();

    public ReduceModifier() { }

    public override int GetValue(int oValue, ModifierKey context)
    {
        return oValue - _amount.GetAmount(context.Context);
    }
}
