using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView _handView;
    [SerializeField] private List<CardView> _viewsInPlay = new();
    [SerializeField] private CardPileView _drawPileView;
    [SerializeField] private CardPileView _discardPileView;
    [SerializeField] private CardPileView _exhaustedPileView;

    [field: SerializeField] public Color PlayableColor { get; private set; }
    [field: SerializeField] public Color HighlightColor { get; private set; }

    [field: SerializeField] public Transform PlayCardPosition { get; private set; }

    private List<Card> _drawPile = new();
    private List<Card> _discardPile = new();
    private List<Card> _hand = new();
    private List<Card> _exhaustPile = new();
    private List<Card> _cardsInPlay = new();


    public const int MAX_HAND_SIZE = 10;

    void OnEnable()
    {
        _drawPileView.SetUp(_drawPile);
        _discardPileView.SetUp(_discardPile);
        _exhaustedPileView.SetUp(_exhaustPile);

        ActionSystem.AttachPerformer<AddCardsToHandGA>(AddCardsToHandPerformer);
        ActionSystem.AttachPerformer<AddCardsToDeckGA>(AddCardsToDeckPerformer);
        ActionSystem.AttachPerformer<AddCardsToDiscardGA>(AddCardsToDiscardPerformer);
        ActionSystem.AttachPerformer<AutoTargetEffectGA>(AutoTargetEffectPerformer);
        ActionSystem.AttachPerformer<CycleGA>(CyclePerformer);
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerformer);
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<DiscardCardsGA>(DiscardCardsPerformer);
        ActionSystem.AttachPerformer<DiscardCardGA>(DiscardCardPerformer);
        ActionSystem.AttachPerformer<ExhaustCardGA>(ExhaustCardPerformer);
        ActionSystem.AttachPerformer<ExhaustCardsGA>(ExhaustCardsPerformer);
        ActionSystem.AttachPerformer<HandCardThrobGA>(HandCardThrobPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.AttachPerformer<RetainGA>(RetainPerformer);
        ActionSystem.AttachPerformer<ShuffleGA>(ShufflePerformer);
        ActionSystem.AttachPerformer<UnspotlightCardGA>(UnspotlightCardPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AddCardsToHandGA>();
        ActionSystem.DetachPerformer<AddCardsToDeckGA>();
        ActionSystem.DetachPerformer<AddCardsToDiscardGA>();
        ActionSystem.DetachPerformer<AutoTargetEffectGA>();
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<DiscardCardsGA>();
        ActionSystem.DetachPerformer<DiscardCardGA>();
        ActionSystem.DetachPerformer<ExhaustCardGA>();
        ActionSystem.DetachPerformer<ExhaustCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.DetachPerformer<RetainGA>();
        ActionSystem.DetachPerformer<ShuffleGA>();
        ActionSystem.DetachPerformer<UnspotlightCardGA>();
    }

    public void SetUp(List<Card> deckData)
    {
        _drawPile.AddRange(deckData);

        foreach (Card card in _drawPile)
        {
            SetCardReactions(card);
        }

        UpdateUI();
    }

    public void UpdateCardHoverView()
    {
        _handView.UpdateCardHoverView();
    }

    private IEnumerator AddCardsToHandPerformer(AddCardsToHandGA addCardsToHandGA)
    {
        foreach (Card card in addCardsToHandGA.Cards.Keys)
        {
            CardView cardView = GetCardViewForEffect(card, 0.15f);

            yield return new WaitForSeconds(0.65f);

            _hand.Add(card);
            SetCardReactions(card);

            yield return _handView.AddCard(cardView, addCardsToHandGA.Cards[card]);

            cardView.SortingGroup.sortingOrder--;
            DynamicViewsSystem.Instance.UpdateDynamicValues();
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

            int index = RNG.Random.Next(0, _drawPile.Count);

            _drawPile.Insert(index, card);
            SetCardReactions(card);
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
            SetCardReactions(card);
        }
    }

    private IEnumerator AutoTargetEffectPerformer(AutoTargetEffectGA performAutoTargetEffectGA)
    {
        GameAction gameAction = performAutoTargetEffectGA.Effect.GetGameAction(performAutoTargetEffectGA.Context);
        ActionSystem.Instance.AddReaction(gameAction);
        yield return null;
    }


    private IEnumerator CyclePerformer(CycleGA cycleGA)
    {
        foreach (Card card in cycleGA.Cards)
        {
            DiscardCardGA discardCardGA = new(card);
            ActionSystem.Instance.AddReaction(discardCardGA);
        }

        DrawCardsGA drawCardsGA = new(cycleGA.Cards.Count);
        ActionSystem.Instance.AddReaction(drawCardsGA);
        yield return null;
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        if (_drawPile.Count == 0 && drawCardsGA.amount > 0)
        {
            yield return RefillDeck();

            ShuffleGA shuffleGA = new();
            ActionSystem.Instance.AddReaction(shuffleGA);
        }

        for (int i = 0; i < drawCardsGA.amount; i++)
        {
            DrawCardGA drawCardGA = new(i != drawCardsGA.amount - 1);
            ActionSystem.Instance.AddReaction(drawCardGA);
        }
    }

    private IEnumerator DrawCardPerformer(DrawCardGA drawCardGA)
    {
        yield return DrawCard(drawCardGA);

        if (_drawPile.Count == 0 && drawCardGA.ExpectAnotherDraw)
        {
            yield return RefillDeck();

            ShuffleGA shuffleGA = new();
            ActionSystem.Instance.AddReaction(shuffleGA);
            DynamicViewsSystem.Instance.UpdateDynamicValues();
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
            if (!discardCardsGA.ForEndOfTurn || !card.GetRetain())
            {
                DiscardCardGA discardCardGA = new(card, discardCardsGA.ForEndOfTurn);
                ActionSystem.Instance.AddReaction(discardCardGA);
            }
            else if(card.GetRetain())
            {
                
            }
        }

        yield return null;
    }

    private IEnumerator DiscardCardPerformer(DiscardCardGA discardCardGA)
    {
        CardView cardView = discardCardGA.TargetView;

        if (cardView == null)
        {
            cardView = _handView.RemoveCard(discardCardGA.Target);

            if (cardView == null)
            {
                if (SpotlightSystem.Instance.SpotlightCardViews.Any(cv => cv.Card == discardCardGA.Target))
                {
                    cardView = SpotlightSystem.Instance.SpotlightCardViews.Find(cv => cv.Card == discardCardGA.Target);
                    SpotlightSystem.Instance.RemoveSpotlightCardView(cardView);
                }
                else if (_cardsInPlay.Contains(discardCardGA.Target))
                {
                    cardView = GetInPlayCardView(discardCardGA.Target, true);
                    _cardsInPlay.Remove(discardCardGA.Target);
                }
            }

            _hand.Remove(discardCardGA.Target);
        }

        yield return DiscardCard(cardView);
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        CardView cardView = GetCardView(playCardGA.card);

        EffectContext context = playCardGA.GetEffectContext();

        if (playCardGA.PayCost)
        {
            int manaCost = playCardGA.card.GetDynamicManaValue(context);
            SpendManaGA spendManaGA = new(manaCost);
            ActionSystem.Instance.AddReaction(spendManaGA);
        }

        _viewsInPlay.Add(cardView);
        _cardsInPlay.Add(playCardGA.card);
        _hand.Remove(playCardGA.card);

        cardView.transform.DOKill();
        cardView.transform.DOMove(PlayCardPosition.position, 0.15f);
        yield return cardView.transform.DORotate(Vector3.zero, 0.15f).WaitForCompletion();

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
                yield return effectWrapper.WaitForUserInput(context);

            AutoTargetEffectGA gameAction = new AutoTargetEffectGA(context, effectWrapper);
            ActionSystem.Instance.AddReaction(gameAction);
        }

        if (!playCardGA.card.ExhuastOnUse)
        {
            DiscardCardGA discardCardGA = new DiscardCardGA(playCardGA.card);
            ActionSystem.Instance.AddReaction(discardCardGA);
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

        _cardsInPlay.Remove(cardView.Card);
        _exhaustPile.Add(cardView.Card);

        SetCardReactions(exhaustCardGA.Card);
        UpdateUI();
    }

    private IEnumerator ExhaustCardsPerformer(ExhaustCardsGA exhaustCardsGA)
    {
        foreach (Card card in exhaustCardsGA.Cards)
        {
            ExhaustCardGA exhaustCardGA = new(card);
            ActionSystem.Instance.AddReaction(exhaustCardGA);
        }

        yield return null;
    }

    private IEnumerator HandCardThrobPerformer(HandCardThrobGA handCardThrobGA)
    {
        yield return HandCardThrob(handCardThrobGA.Card);
    }

    private IEnumerator RetainPerformer(RetainGA retainGA)
    {
        yield break;
    }

    private IEnumerator ShufflePerformer(ShuffleGA shuffleGA)
    {
        _drawPile.Shuffle();
        yield return null;
    }

    private IEnumerator UnspotlightCardPerformer(UnspotlightCardGA unspotlightCardGA)
    {
        int index = _hand.IndexOf(unspotlightCardGA.Target.Card);

        if (SpotlightSystem.Instance.SpotlightCardViews.Contains(unspotlightCardGA.Target) && index > -1)
        {
            yield return _handView.AddCard(unspotlightCardGA.Target, index);
            SpotlightSystem.Instance.RemoveSpotlightCardView(unspotlightCardGA.Target);
        }
    }

    private IEnumerator DrawCard(DrawCardGA drawCardGA)
    {
        Card card = _drawPile.Draw();

        if (card == null)
        {
            SpeechBubbleGA speechBubbleGA = new("No more cards left to draw!", new() { HeroSystem.Instance.HeroViews[0] }, 5, 0);
            ActionSystem.Instance.AddReaction(speechBubbleGA);
            yield break;
        }

        UpdateUI();

        _hand.Add(card);
        drawCardGA.SetCardDrawn(card);

        CardView cardView = CardViewCreator.Instance.CreateCardView(card, _drawPileView.transform.position, _drawPileView.transform.rotation);
        yield return _handView.AddCard(cardView);
        SetCardReactions(card);
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        if (cardView != null)
        {
            _discardPile.Add(cardView.Card);
            SetCardReactions(cardView.Card);

            cardView.transform.DOScale(Vector3.zero, 0.15f);
            Tween tween = cardView.transform.DOMove(_discardPileView.transform.position, 0.15f);
            yield return tween.WaitForCompletion();

            UpdateUI();

            Destroy(cardView.gameObject);
        }
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
            SetCardReactions(card);

            UpdateUI();
        }

        yield return null;
    }

    private CardView GetCardView(Card card)
    {
        CardView cardView = null;

        if (_drawPile.Contains(card))
        {
            _drawPile.Remove(card);
            _drawPileView.UpdateUI();
        }
        else if (_discardPile.Contains(card))
        {
            _discardPile.Remove(card);
            _discardPileView.UpdateUI();
        }
        else if (_exhaustPile.Contains(card))
        {
            _exhaustPile.Remove(card);
            _exhaustedPileView.UpdateUI();
        }
        else if (_hand.Contains(card))
        {
            cardView = _handView.RemoveCard(card);
            _hand.Remove(card);
        }
        else if (_cardsInPlay.Contains(card))
        {
            cardView = GetInPlayCardView(card, true);
            _cardsInPlay.Remove(card);
        }
        else if (SpotlightSystem.Instance.SpotlightCardViews.Any(cv => cv.Card == card))
        {
            cardView = SpotlightSystem.Instance.SpotlightCardViews.First(cv => cv.Card == card);
            SpotlightSystem.Instance.RemoveSpotlightCardView(cardView);
        }

        return cardView;
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
        else if (_cardsInPlay.Contains(card))
        {
            cardView = GetInPlayCardView(card, true);
            _cardsInPlay.Remove(card);
        }
        else if (SpotlightSystem.Instance.SpotlightCardViews.Any(cv => cv.Card == card))
        {
            cardView = SpotlightSystem.Instance.SpotlightCardViews.First(cv => cv.Card == card);
            SpotlightSystem.Instance.RemoveSpotlightCardView(cardView);
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
        SpotlightSystem.Instance.UpdateCardViews();
    }

    public List<Card> GetDrawPile()
    {
        return new(_drawPile);
    }

    public List<Card> GetDiscardPile()
    {
        return new(_discardPile);
    }

    public List<Card> GetHand()
    {
        return new(_hand);
    }

    public List<Card> GetExhaustPile()
    {
        return new(_exhaustPile);
    }

    public int GetHandIndex(Card card)
    {
        return _hand.IndexOf(card);
    }

    public List<Card> CardsInHandToLeft(Card card)
    {
        int index = GetHandIndex(card);

        if (index == -1)
            return null;

        return _hand.GetRange(0, index);
    }

    public int GetPresentInHandCount(List<Card> cards)
    {
        return cards.Sum(c => _hand.Contains(c) ? 1 : 0);
    }

    public CardView GetInPlayCardView(Card card, bool remove)
    {
        CardView cardView = _viewsInPlay.Find(cv => cv.Card == card);

        if (cardView != null)
        {
            if (remove)
            {
                _cardsInPlay.Remove(card);
                _viewsInPlay.Remove(cardView);
            }

            return cardView;
        }

        return null;
    }

    public CardView GetViewFromHand(Card card, bool remove = true)
    {
        CardView cardView = _handView.RemoveCard(card);

        if (cardView != null)
        {
            if (remove)
                _hand.Remove(card);

            return cardView;
        }

        return null;
    }

    public IEnumerator ReturnViewToHand(CardView cardView)
    {
        int index = _hand.IndexOf(cardView.Card);
        yield return _handView.AddCard(cardView, index);

        UpdateCardViews();
    }

    public int GetCardCount()
    {
        return _hand.Count;
    }

    private void SetCardReactions(Card card)
    {
        card.UnsubscribeAllReactions();

        card.SubscribeAnywhereReactions();

        if (_hand.Contains(card))
        {
            card.SubscribeInHand();
            return;
        }
    }

    public IEnumerator HandCardThrob(Card card)
    {
        yield return _handView.ApplyCardThrob(card, 0.5f);
    }
}
