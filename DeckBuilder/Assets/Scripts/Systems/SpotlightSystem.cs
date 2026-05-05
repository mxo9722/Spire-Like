using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpotlightSystem : Singleton<SpotlightSystem>
{

    public List<CardView> SpotlightCardViews { get; private set; } = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpotlightCardGA>(SpotlightCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpotlightCardGA>();
    }

    private IEnumerator SpotlightCardPerformer(SpotlightCardGA spotlightCardViewGA)
    {
        yield return SpotlightCard(spotlightCardViewGA.Target);
    }

    public IEnumerator SpotlightCard(CardView cardView)
    {
        Vector3 center = Vector3.zero;
        center.x = Camera.main.transform.position.x;
        center.y = Camera.main.transform.position.y;

        int prevID = cardView.SortingGroup.sortingLayerID;
        cardView.SortingGroup.sortingLayerID = SortingLayer.layers.Last().id;

        cardView.transform.DOKill();
        cardView.transform.DOMove(center, 0.15f);
        cardView.transform.DORotate(Vector3.zero, 0.15f);

        yield return new WaitForSeconds(0.3f);

        cardView.SortingGroup.sortingLayerID = prevID;
        SpotlightCardViews.Add(cardView);
    }

    public void RemoveSpotlightCardView(CardView cardView)
    {
        SpotlightCardViews.Remove(cardView);
    }

    public void UpdateCardViews()
    {
        foreach(CardView cardView in SpotlightCardViews)
        {
            cardView.UpdateDynamicDescription(new(cardView.Card.GetOwnerView()));
            cardView.UpdateGlow();
        }
    }
}
