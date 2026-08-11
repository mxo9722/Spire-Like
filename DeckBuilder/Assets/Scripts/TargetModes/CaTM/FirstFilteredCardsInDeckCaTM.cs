using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class FirstFilteredCardsInDeckCaTM : CardTargetMode
{
    [SerializeReference, SR] private Quantity _cardCount = new SetQ();
    [SerializeReference, SR] private List<CardFilter> _filters = new();
    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> drawPile = CardSystem.Instance.GetDrawPile();

        int countNeeded = _cardCount.GetAmount(context);
        int curCount = 0;

        List<Card> targets = new();

        foreach(Card card in drawPile)
        {
            //This check is here so that when the count is equal to 0 it stops right away
            if (curCount == countNeeded)
                break;

            if (_filters.TrueForAll(t => t.TestTarget(context, card)))
            {
                targets.Add(card);
                curCount++;
            }
        }

        return targets;
    }
}
