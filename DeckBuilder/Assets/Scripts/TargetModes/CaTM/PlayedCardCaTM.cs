using System.Collections.Generic;
using UnityEngine;

public class PlayedCardCaTM : CardTargetMode
{
    public override List<Card> GetTargets(EffectContext context)
    {
        return new() { context.PlayedCard };
    }
}
