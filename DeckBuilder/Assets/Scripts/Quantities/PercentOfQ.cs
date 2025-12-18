using SerializeReferenceEditor;
using UnityEngine;

public class PercentOfQ : Quantity
{
    [SerializeField, Range(0, 1)] private float _percent;
    [SerializeReference, SR] private Quantity _value = new SetQ();


    public override int GetAmount(EffectContext effectContext)
    {
        int ret = Mathf.RoundToInt(_value.GetAmount(effectContext) * _percent);
        return ret;
    }

    public override int GetStaticAmount()
    {
        int ret = Mathf.RoundToInt(_value.GetStaticAmount() * _percent);
        return ret;
    }
}
