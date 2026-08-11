using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class CopiesCaTM : CardTargetMode
{
    [SerializeReference, SR] private CardTargetMode _copySource = new PlayedCardCaTM();
    [SerializeReference, SR] private Quantity _copyCount = new SetQ(1);

    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> cards = _copySource.GetTargets(context);
        int copyCount = _copyCount.GetAmount(context);
        int sourceCount = cards.Count;

        for (int i = 0; i < sourceCount; i++)
        {
            for (int j = 0; j < copyCount; j++)
            {

                cards.Add(new(cards[0]));

            }

            cards.RemoveAt(0);
        }

        return cards;
    }
}
