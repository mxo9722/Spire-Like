using UnityEngine;

public class ExtraBlockCM : CardModifier
{
    public int Amount { get; private set; } = 0;

    public ExtraBlockCM() { }

    public ExtraBlockCM(int amount)
    {
        Amount = amount;
    }

    public override bool CanApply(Card card)
    {
        return card.GetAllStatusEffects().Contains(StatusEffect.BLOCK);
    }

    public override object Clone()
    {
        return new ExtraBlockCM(Amount);
    }

    public override bool TryToCombine(CardModifier cardModifier)
    {
        if(cardModifier is ExtraBlockCM extraBlockCM)
        {
            Amount += extraBlockCM.Amount;
            return true;
        }

        return false;
    }
}
