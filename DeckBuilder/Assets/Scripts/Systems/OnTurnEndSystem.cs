using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class OnTurnEndSystem : Singleton<OnTurnEndSystem>
{

    public CardView SpotlightCardView { get; private set; } = null;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DiscardCardGA>(this, PreDiscardCardHandler, ReactionTiming.PRE);

        ActionSystem.AttachPerformer<SpotlightCardGA>(SpotlightCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DiscardCardGA>(this, PreDiscardCardHandler, ReactionTiming.PRE);

        ActionSystem.DetachPerformer<SpotlightCardGA>();
    }

    private void PreDiscardCardHandler(DiscardCardGA discardCardGA)
    {
        Card card = discardCardGA.Target;
        CardView cardView = discardCardGA.TargetView;

        if (cardView == null)
        {
            cardView = CardSystem.Instance.RemoveFromHand(card);
            discardCardGA.SetCardView(cardView);
        }


        if (discardCardGA.ForEndOfTurn && card.TurnEndEffect.Count > 0)
        {
            SpotlightCardGA spotlightCardGA = new(cardView);
            ActionSystem.Instance.AddReaction(spotlightCardGA);

            foreach(AutoTargetEffect effect in card.TurnEndEffect)
            {
                GameAction gameAction = effect.GetGameAction(new EffectContext(card.GetOwnerView(), playedCard: card));
                ActionSystem.Instance.AddReaction(gameAction);
            }
        }
    }

    private IEnumerator SpotlightCardPerformer(SpotlightCardGA spotlightCardViewGA)
    {
        Vector3 center = Vector3.zero;
        center.x = Camera.main.transform.position.x;
        center.y = Camera.main.transform.position.y;

        int prevID = spotlightCardViewGA.Target.SortingGroup.sortingLayerID;
        spotlightCardViewGA.Target.SortingGroup.sortingLayerID = SortingLayer.layers.Last().id;

        spotlightCardViewGA.Target.transform.DOMove(center, 0.15f);
        spotlightCardViewGA.Target.transform.DORotate(Vector3.zero, 0.15f);

        yield return new WaitForSeconds(0.3f);

        spotlightCardViewGA.Target.SortingGroup.sortingLayerID = prevID;
        SpotlightCardView = spotlightCardViewGA.Target;
    }

    public void RemoveSpotlightCardView()
    {
        SpotlightCardView = null;
    }
}
