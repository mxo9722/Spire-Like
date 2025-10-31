using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView _handView;
    [SerializeField] private CardPileView _drawPileView;
    [SerializeField] private CardPileView _discardPileView;

    private List<Card> _drawPile = new();
    private List<Card> _discardPile = new();
    private List<Card> _hand = new();
    private List<Card> _exhaustPile = new();


    void OnEnable()
    {
        _drawPileView.SetUp(_drawPile);
        _discardPileView.SetUp(_discardPile);

        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.AttachPerformer<ExhaustCardGA>(ExhaustCardPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.DetachPerformer<ExhaustCardGA>();
    }

    public void SetUp(List<CardData> deckData)
    {
        foreach(CardData cardData in deckData)
        {
            Card card = new(cardData);
            _drawPile.Add(card);
        }
        UpdateUI();
        _drawPile.Shuffle();
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.amount,_drawPile.Count);
        int notDrawnAmount = drawCardsGA.amount - actualAmount;
        for(int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        if(notDrawnAmount > 0)
        {
            yield return RefillDeck();

            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardallCardsGA)
    {
        foreach (Card card in _hand)
        {
            CardView cardView = _handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        _hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        _hand.Remove(playCardGA.card);

        CardView cardView = _handView.RemoveCard(playCardGA.card);

        if (!playCardGA.card.ExhuastOnUse)
        {
            yield return DiscardCard(cardView);
        }
        else
        {
            cardView.transform.DOMove(Vector3.zero, 0.15f);
        }

        SpendManaGA spendManaGA = new(playCardGA.card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if(playCardGA.ManualTarget != null)
        {
            PerformEffectsGA performEffectsGA = new PerformEffectsGA(playCardGA.card.ManualTargetEffect, playCardGA.ManualTarget);
            ActionSystem.Instance.AddReaction(performEffectsGA);
        }

        //Perform effects
        foreach (AutoTargetEffect effectWrapper in playCardGA.card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectsGA performEffectsGA = new PerformEffectsGA(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectsGA);
        }

        if (playCardGA.card.ExhuastOnUse)
        {
            ExhaustCardGA exhaustCardGA = new(cardView);
            ActionSystem.Instance.AddReaction(exhaustCardGA);
        }
    }

    private IEnumerator ExhaustCardPerformer(ExhaustCardGA exhaustCardGA)
    {
        Tweener moveCardTweener = exhaustCardGA.CardView.transform.DOMove(Vector3.zero, 0.15f);
        exhaustCardGA.CardView.transform.DORotate(Vector3.zero, 0.15f);

        yield return moveCardTweener.WaitForCompletion();

        yield return exhaustCardGA.CardView.ActivateBurnVFX();

        _exhaustPile.Add(exhaustCardGA.CardView.Card);
    }

    private IEnumerator DrawCard()
    {
        Card card = _drawPile.Draw();
        UpdateUI();

        _hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, _drawPileView.transform.position, _drawPileView.transform.rotation);
        yield return _handView.AddCard(cardView);
    }
    
    private IEnumerator DiscardCard(CardView cardView)
    {

        _discardPile.Add(cardView.Card);
        cardView.transform.DOScale(Vector3.zero,0.15f);
        Tween tween = cardView.transform.DOMove(_discardPileView.transform.position, 0.15f);
        yield return tween.WaitForCompletion();

        UpdateUI();

        Destroy(cardView.gameObject);
    }

    private IEnumerator RefillDeck()
    {
        int cardTransferTotal = _discardPile.Count;

        float refillDeckLength = 1;
        float lerpLength = refillDeckLength / cardTransferTotal;

        while (_discardPile.Count > 0)
        {
            yield return new WaitForSeconds(lerpLength);

            Card card = _discardPile.Draw();
            _drawPile.Add(card);
            UpdateUI();
        }

        _drawPile.Shuffle();

        yield return null;
    }


    private void UpdateUI()
    {
        _drawPileView.UpdateUI();
        _discardPileView.UpdateUI();
    }
}
