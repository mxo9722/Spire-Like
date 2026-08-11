using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AdjacentInHandCaTM : CardTargetMode, INeedsUserInput
{

    [SerializeReference, SR] private CardTargetMode _baseTM;

    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> baseTargets = _baseTM.GetTargets(context);
        List<Card> adjacent = new();

        List<Card> hand = CardSystem.Instance.GetHand();

        foreach(Card card in baseTargets)
        {
            int index = hand.IndexOf(card);

            if (index == -1)
                continue;

            if (index > 0)
                adjacent.Add(hand[index - 1]);
            if (index < hand.Count - 1)
                adjacent.Add(hand[index + 1]);
        }

        adjacent = adjacent.Except(baseTargets).Distinct().ToList();

        return adjacent;
    }

    public IEnumerator WaitForUserInput(EffectContext context)
    {
        if(_baseTM is INeedsUserInput iNeedsUserInput)
        {
            yield return iNeedsUserInput.WaitForUserInput(context);
        }

        yield break;
    }
}
