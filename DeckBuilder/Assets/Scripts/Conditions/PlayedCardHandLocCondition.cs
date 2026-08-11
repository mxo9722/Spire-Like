using UnityEngine;

public class PlayedCardHandLocCondition : Condition
{
    [SerializeField] private InHandLocFilter _handLoc = new();

    protected override bool IsConditionMet(EffectContext context)
    {
        return _handLoc.TestTarget(context, context.PlayedCard);
    }
}
