using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddCardToDeckEA : EventAction
{
    [SerializeField] private List<CardData> _cards = new();

    public override IEnumerator Invoke()
    {
        foreach(var card in _cards)
            RunSystem.Instance.AddCard(new(card));

        List<CardView> cardViews = new();

        foreach (var card in _cards)
        {
            var cardView = CardViewCreator.Instance.CreateCardView(new(card), Vector3.zero, Quaternion.identity, true);
            cardView.SortingGroup.sortingLayerID = SortingLayer.layers.Last().id;
            cardViews.Add(cardView);
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1.2f);

        List<Tweener> tweeners = new();

        foreach(CardView cardView in cardViews)
        {
            DOTween.TweensByTarget(cardView.transform)?.ForEach(t => t.Complete());
            var moveTween = cardView.transform.DOMove(TopBarUI.Instance.DeckUIPos, 0.5f);
            var scaleTween = cardView.transform.DOScale(Vector3.zero, 0.5f);
            tweeners.Add(moveTween);
        }

        foreach(var tween in tweeners)
        {
            yield return tween.WaitForCompletion();
        }

        cardViews.ForEach(cv => Object.Destroy(cv.gameObject));
    }
}
