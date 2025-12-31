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
    [SerializeField] private CardPileView _exhaustedPileView;

    [field: SerializeField] public Color PlayableColor { get; private set; }
    [field: SerializeField] public Color HighlightColor { get; private set; }

    private List<Card> _drawPile = new();
    private List<Card> _discardPile = new();
    private List<Card> _hand = new();
    private List<Card> _exhaustPile = new();


    void OnEnable()
    {
        _drawPileView.SetUp(_drawPile);
        _discardPileView.SetUp(_discardPile);
        _exhaustedPileView.SetUp(_exhaustPile);

        ActionSystem.AttachPerformer<AddCardsToHandGA>(AddCardsToHandPerformer);
        ActionSystem.AttachPerformer<AddCardsToDeckGA>(AddCardsToDeckPerformer);
        ActionSystem.AttachPerformer<AddCardsToDiscardGA>(AddCardsToDiscardPerformer);
        ActionSystem.AttachPerformer<AutoTargetEffectGA>(AutoTargetEffectPerformer);
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<DiscardCardsGA>(DiscardCardsPerformer);
        ActionSystem.AttachPerformer<DiscardCardGA>(DiscardCardPerformer);
        ActionSystem.AttachPerformer<ExhaustCardGA>(ExhaustCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AddCardsToHandGA>();
        ActionSystem.DetachPerformer<AddCardsToDeckGA>();
        ActionSystem.DetachPerformer<AddCardsToDiscardGA>();
        ActionSystem.DetachPerformer<AutoTargetEffectGA>();
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<DiscardCardsGA>();
        ActionSystem.DetachPerformer<DiscardCardGA>();
        ActionSystem.DetachPerformer<ExhaustCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    public void SetUp(List<CardData> deckData)
    {
        foreach (CardData cardData in deckData)
        {
            Card card = new(cardData);
            _drawPile.Add(card);
        }
        UpdateUI();
        _drawPile.Shuffle();
    }

    public void UpdateCardHoverView()
    {
        _handView.UpdateCardHoverView();
    }

    private IEnumerator AddCardsToHandPerformer(AddCardsToHandGA addCardsToHandGA)
    {
        foreach (Card card in addCardsToHandGA.Cards)
        {
            CardView cardView = GetCardViewForEffect(card, 0.15f);

            yield return new WaitForSeconds(0.65f);

            _hand.Add(card);

            yield return _handView.AddCard(cardView);
        }
    }

    private IEnumerator AddCardsToDeckPerformer(AddCardsToDeckGA addCardsToDeckGA)
    {
        foreach (Card card in addCardsToDeckGA.Cards)
        {
            CardView cardView = GetCardViewForEffect(card, 0.15f);

            yield return new WaitForSeconds(0.65f);

            cardView.transform.DOMove(_drawPileView.transform.position, 0.15f);
            Tween tween = cardView.transform.DOScale(Vector3.zero, 0.15f);

            yield return tween.WaitForCompletion();

            Destroy(cardView.gameObject);
            _drawPile.Add(card);
        }
    }


    private IEnumerator AddCardsToDiscardPerformer(AddCardsToDiscardGA addCardsToDiscardGA)
    {

        foreach (Card card in addCardsToDiscardGA.Cards)
        {
            CardView cardView = GetCardViewForEffect(card, 0.15f);

            yield return new WaitForSeconds(0.65f);

            cardView.transform.DOMove(_discardPileView.transform.position, 0.15f);
            Tween tween = cardView.transform.DOScale(Vector3.zero, 0.15f);

            yield return tween.WaitForCompletion();

            Destroy(cardView.gameObject);
            _discardPile.Add(card);
        }
    }

    private IEnumerator AutoTargetEffectPerformer(AutoTargetEffectGA performAutoTargetEffectGA)
    {
        GameAction gameAction = performAutoTargetEffectGA.Effect.GetGameAction(performAutoTargetEffectGA.Context);
        ActionSystem.Instance.AddReaction(gameAction);
        yield return null;
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.amount, _drawPile.Count);
        int notDrawnAmount = drawCardsGA.amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        if (notDrawnAmount > 0)
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
        List<Card> cards = new(_hand);
        cards.Reverse();
        DiscardCardsGA discardCardGA = new(cards, discardallCardsGA.ForEndOfTurn);
        ActionSystem.Instance.AddReaction(discardCardGA);

        yield return null;
    }

    private IEnumerator DiscardCardsPerformer(DiscardCardsGA discardCardsGA)
    {
        foreach (Card card in discardCardsGA.Targets)
        {
            DiscardCardGA discardCardGA = new(card, discardCardsGA.ForEndOfTurn);
            ActionSystem.Instance.AddReaction(discardCardGA);
        }

        yield return null;
    }

    private IEnumerator DiscardCardPerformer(DiscardCardGA discardCardGA)
    {
        CardView cardView = discardCardGA.TargetView;

        if (cardView == null)
        {
            cardView = _handView.RemoveCard(discardCardGA.Target);
            _hand.Remove(discardCardGA.Target);
        }

        yield return DiscardCard(cardView);

    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        SpendManaGA spendManaGA = new(playCardGA.card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        HeroView heroView = HeroSystem.Instance.HeroView;
        EffectContext context = new(heroView, playCardGA.ManualLaneTarget, playCardGA.ManualEnemyTarget);

        if (playCardGA.card.ManualTargetEffect != null)
        {
            if (playCardGA.ManualEnemyTarget != null)
            {
                PerformEffectsGA performEffectsGA = new PerformEffectsGA(context, playCardGA.card.ManualTargetEffect, playCardGA.ManualEnemyTarget);
                ActionSystem.Instance.AddReaction(performEffectsGA);
            }
            else if (playCardGA.ManualLaneTarget != null)
            {
                PerformEffectsGA performEffectsGA = new PerformEffectsGA(context, playCardGA.card.ManualTargetEffect, playCardGA.ManualLaneTarget);
                ActionSystem.Instance.AddReaction(performEffectsGA);
            }
        }

        //Perform effects
        foreach (AutoTargetEffect effectWrapper in playCardGA.card.OtherEffects)
        {
            if (effectWrapper.RequiresUserInput())
                yield return effectWrapper.WaitForUserInput();

            AutoTargetEffectGA gameAction = new AutoTargetEffectGA(context, effectWrapper);
            ActionSystem.Instance.AddReaction(gameAction);
        }

        if (!playCardGA.card.ExhuastOnUse)
        {
            _hand.Remove(playCardGA.card);
            CardView cardView = _handView.RemoveCard(playCardGA.card);

            yield return DiscardCard(cardView);
        }
        else
        {
            ExhaustCardGA exhaustCardGA = new(playCardGA.card);
            ActionSystem.Instance.AddReaction(exhaustCardGA);
        }
    }

    private IEnumerator ExhaustCardPerformer(ExhaustCardGA exhaustCardGA)
    {
        CardView cardView = GetCardViewForEffect(exhaustCardGA.Card, 0.15f);

        Tweener moveCardTweener = cardView.transform.DOMove(Vector3.zero, 0.15f);
        cardView.transform.DORotate(Vector3.zero, 0.15f);

        yield return moveCardTweener.WaitForCompletion();

        yield return cardView.ActivateExhaustVFX();

        _exhaustPile.Add(cardView.Card);
        UpdateUI();
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
        cardView.transform.DOScale(Vector3.zero, 0.15f);
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

    private CardView GetCardViewForEffect(Card card, float duration)
    {
        Transform pileView = default;
        CardView cardView = null;

        if (_drawPile.Contains(card))
        {
            pileView = _drawPileView.transform;

            _drawPile.Remove(card);
            _drawPileView.UpdateUI();
        }
        else if (_discardPile.Contains(card))
        {
            pileView = _discardPileView.transform;

            _discardPile.Remove(card);
            _discardPileView.UpdateUI();
        }
        else if (_exhaustPile.Contains(card))
        {
            pileView = _exhaustedPileView.transform;

            _exhaustPile.Remove(card);
            _exhaustedPileView.UpdateUI();
        }
        else if (_hand.Contains(card))
        {
            cardView = _handView.RemoveCard(card);
            _hand.Remove(card);
        }

        if (cardView == null)
        {
            if (pileView != default)
                cardView = CardViewCreator.Instance.CreateCardView(card, pileView.position, pileView.rotation, true);
            else
                cardView = CardViewCreator.Instance.CreateCardView(card, Vector3.zero, Quaternion.identity, true);
        }
        cardView.transform.DOMove(Vector3.zero, duration);
        cardView.transform.DORotate(Vector3.zero, duration);
        cardView.SortingGroup.sortingOrder++;

        return cardView;
    }


    private void UpdateUI()
    {
        _drawPileView.UpdateUI();
        _discardPileView.UpdateUI();
        _exhaustedPileView.UpdateUI();
    }

    public void UpdateCardViews()
    {
        _handView.UpdateCardViews();
    }

    public List<Card> GetDrawPile()
    {
        return new(_drawPile);
    }

    public List<Card> GetExhaustPile()
    {
        return new(_exhaustPile);
    }

    public CardView RemoveFromHand(Card card) 
    {
        if (_hand.Contains(card))
        {
            CardView cardView = _handView.RemoveCard(card);
            _hand.Remove(card);
            return cardView;
        }
        return null;
    }
}
