using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RandomCaTM : CardTargetMode
{
    [SerializeReference, SR] private List<CardFilter> _filters;
    [SerializeReference, SR] private Quantity _numberOfCards = new SetQ(1);

    protected abstract List<Card> CardSource { get; }

    public override List<Card> GetTargets(EffectContext context)
    {
        int quantToRemove = _numberOfCards.GetAmount(context);

        List<Card> cards = CardSource;
        cards.RemoveAll(c => !_filters.TargetIsValid(c, context));

        quantToRemove = Mathf.Max(0, cards.Count - quantToRemove);

        if (quantToRemove == 0) return cards;
        if (quantToRemove == cards.Count)
        {
            cards.Clear();
            return cards;
        }

        for(int i = 0; i < quantToRemove; i++)
        {
            int index = RNG.Random.Next(cards.Count);
            cards.RemoveAt(index);
        }

        return cards;
    }
}

public class RandomFromHandCaTM : RandomCaTM
{
    protected override List<Card> CardSource => CardSystem.Instance.GetHand();
}

public class RandomFromDeckCaTM : RandomCaTM
{
    protected override List<Card> CardSource => CardSystem.Instance.GetDrawPile();
}

public class RandomFromDiscardCaTM : RandomCaTM
{
    protected override List<Card> CardSource => CardSystem.Instance.GetDiscardPile();
}
