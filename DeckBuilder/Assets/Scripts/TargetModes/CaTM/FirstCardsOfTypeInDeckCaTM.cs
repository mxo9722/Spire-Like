using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class FirstCardsOfTypeInDeckCaTM : CardTargetMode
{
    [SerializeReference, SR] private Quantity CardCount = new SetQ();
    [SerializeField] private List<CardData> Cards;

    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> drawPile = CardSystem.Instance.GetDrawPile();

        int countNeeded = CardCount.GetAmount(context);
        int curCount = 0;

        List<Card> targets = new();

        foreach(Card card in drawPile)
        {
            //This check is here so that when the count is equal to 0 it stops right away
            if (curCount == countNeeded)
                break;

            if (Cards.Contains(card.data))
            {
                targets.Add(card);
                curCount++;
            }
        }

        return targets;
    }
}
