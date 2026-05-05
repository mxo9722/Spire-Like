using SerializeReferenceEditor;
using UnityEngine;

public class SetModifier : Modifier
{
    [SerializeReference, SR] private Quantity _value = new SetQ();

    public override int GetValue(int oValue, ModifierKey key)
    {
        return _value.GetAmount(key.Context);
    }
}
