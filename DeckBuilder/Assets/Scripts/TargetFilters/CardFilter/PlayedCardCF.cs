using UnityEngine;

public class PlayedCardCF : CardFilter
{
    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        if (context == null || context.PlayedCard == null)
            return false;

        return context.PlayedCard == target;
    }
}
