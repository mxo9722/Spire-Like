using SerializeReferenceEditor;
using UnityEngine;

public class RandomQ : Quantity
{

    [SerializeReference, SR] private Quantity _minInclusive = new SetQ();
    [SerializeReference, SR] private Quantity _maxExclusive = new SetQ();

    public override int GetAmount(EffectContext effectContext)
    {
        int min = _minInclusive.GetAmount(effectContext);
        int max = _maxExclusive.GetAmount(effectContext);

        int value = RNG.Random.Next(min, max);

        return value;
    }

    public override int GetStaticAmount()
    {
        int min = _minInclusive.GetStaticAmount();
        int max = _maxExclusive.GetStaticAmount();

        int value = RNG.Random.Next(min, max);

        return value;
    }
}
