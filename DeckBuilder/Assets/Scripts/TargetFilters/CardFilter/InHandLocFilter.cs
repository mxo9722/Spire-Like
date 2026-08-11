using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InHandLocFilter : CardFilter
{
    public enum HandLoc
    {
        LEFT_MOST,
        LEFT_SIDE,
        CENTER_MOST,
        RIGHT_MOST,
        RIGHT_SIDE
    }
    [SerializeField] private HandLoc _handLocation = HandLoc.LEFT_MOST;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        if (context.PlayedCard == target && context.PlayedHandIndex != -1)
            return TestIndex(context.PlayedHandIndex, context.PlayedHandSize);

        List<Card> hand = CardSystem.Instance.GetHand();
        int index = hand.IndexOf(target);

        return TestIndex(index, hand.Count);
    }

    private bool TestIndex(int index,int size)
    {

        if (index == -1)
            return false;

        float half = (size - 1) / 2.0f;

        switch (_handLocation)
        {
            case HandLoc.LEFT_MOST:
                return index == 0;
            case HandLoc.LEFT_SIDE:
                return index <= half;
            case HandLoc.CENTER_MOST:
                return Mathf.Abs(index - half) < 1;
            case HandLoc.RIGHT_MOST:
                return index == size - 1;
            case HandLoc.RIGHT_SIDE:
                return index >= half;
        }
        return false;
    }
}
