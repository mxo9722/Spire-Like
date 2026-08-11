using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public abstract class WholeCollectionCaTM : CardTargetMode
{
    [SerializeReference, SR] private List<CardFilter> _filters = new();

    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> collection = GetCollection();

        collection = new(collection.ApplyFilters(_filters, context));

        return collection;
    }

    protected abstract List<Card> GetCollection();
}


public class WholeHandCaTM : WholeCollectionCaTM
{
    protected override List<Card> GetCollection() => CardSystem.Instance.GetHand();
}

public class WholeDrawPileCaTM : WholeCollectionCaTM
{
    protected override List<Card> GetCollection() => CardSystem.Instance.GetDrawPile();
}

public class WholeDiscardPileCaTM : WholeCollectionCaTM
{
    protected override List<Card> GetCollection() => CardSystem.Instance.GetDiscardPile();
}

public class WholeExhuastPileCaTM : WholeCollectionCaTM
{
    protected override List<Card> GetCollection() => CardSystem.Instance.GetExhaustPile();
}

public class CardsFromSpecificSourcesCaTM : WholeCollectionCaTM
{

    [SerializeField] private bool _includeHand = true;
    [SerializeField] private bool _includeDrawPile = true;
    [SerializeField] private bool _includeDiscardPile = true;
    [SerializeField] private bool _includeExhaustPile = true;

    protected override List<Card> GetCollection()
    {
        List<Card> cards = new();

        if (_includeHand)
            cards.AddRange(CardSystem.Instance.GetHand());
        if (_includeDrawPile)
            cards.AddRange(CardSystem.Instance.GetDrawPile());
        if (_includeDiscardPile)
            cards.AddRange(CardSystem.Instance.GetDiscardPile());
        if (_includeExhaustPile)
            cards.AddRange(CardSystem.Instance.GetExhaustPile());

        return cards;
    }
}
