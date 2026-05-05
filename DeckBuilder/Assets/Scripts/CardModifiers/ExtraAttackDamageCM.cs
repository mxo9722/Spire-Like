using UnityEngine;

public class ExtraAttackDamageCM : CardModifier
{
    [field: SerializeField] public int DamageBonus { get; private set; } = 0;

    public ExtraAttackDamageCM(int damageBonus)
    {
        DamageBonus = damageBonus;
    }

    public override bool CanApply(Card card)
    {
        return card.Type == CardType.ATTACK;
    }

    public override bool TryToCombine(CardModifier cardModifier)
    {
        if(cardModifier is ExtraAttackDamageCM other)
        {
            DamageBonus += other.DamageBonus;
            return true;
        }

        return false;
    }

    public override object Clone()
    {
        return new ExtraAttackDamageCM(DamageBonus);
    }
}
