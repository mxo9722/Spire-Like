using System.Collections.Generic;
using UnityEngine;

public class CycleEffect : CardTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        CycleGA cycleGA = new(cardTargets);
        return cycleGA;
    }
}
