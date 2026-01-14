using UnityEngine;

public class IsTypeCF : CardFilter
{
    [SerializeField] private CardType _cardType;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        return target.Type == _cardType;
    }
}
