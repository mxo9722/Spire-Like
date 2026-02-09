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
        Vector3 center = Vector3.zero;
        center.x = Camera.main.transform.position.x;
        center.y = Camera.main.transform.position.y;

        int prevID = spotlightCardViewGA.Target.SortingGroup.sortingLayerID;
        spotlightCardViewGA.Target.SortingGroup.sortingLayerID = SortingLayer.layers.Last().id;

        spotlightCardViewGA.Target.transform.DOKill();
        spotlightCardViewGA.Target.transform.DOMove(center, 0.15f);
        spotlightCardViewGA.Target.transform.DORotate(Vector3.zero, 0.15f);

        yield return new WaitForSeconds(0.3f);

        spotlightCardViewGA.Target.SortingGroup.sortingLayerID = prevID;
        SpotlightCardViews.Add(spotlightCardViewGA.Target);
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
