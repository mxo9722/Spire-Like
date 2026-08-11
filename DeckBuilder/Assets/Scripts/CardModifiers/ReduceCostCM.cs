using UnityEngine;

public class ReduceCostCM : CardModifier
{
    public int Reduction { get; private set; }

    public ReduceCostCM(int reduction)
    {
        Reduction = reduction;
    }

    public override bool CanApply(Card card)
    {
        return true;
    }

    public override object Clone()
    {
        return new ReduceCostCM(Reduction);
    }

    public override bool TryToCombine(CardModifier cardModifier)
    {
        if(cardModifier is ReduceCostCM reduceCardCM)
        {
            Reduction += reduceCardCM.Reduction;
            return true;
        }

        return false;
    }
}
