using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CardRewardView : MonoBehaviour
{

    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private SortingLayer _cardSortingLayer;

    private List<CardView> _cards = new();
    private Action<CardView> _action;

    public void SetUp(CardReward cardReward, Action<CardView> action)
    {
        _action = action;

        Spline spline = _splineContainer.Spline;

        _cards = new();

        List<Card> cards = cardReward.Cards;

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = new(cards[i]);

            float p = (i + 1.00f) / (cards.Count+1.00f);

            Vector3 position = spline.EvaluatePosition(p);

            CardView cardView = CardViewCreator.Instance.CreateCardView(card, position, Quaternion.identity, true);

            cardView.HideGlow();

            cardView.SetSortingOrder(10);

            cardView.OnButtonPressed += action;

            _cards.Add(cardView);

            cardView.transform.parent = _wrapper.transform;
            cardView.SortingGroup.sortingLayerID = SortingLayer.layers[3].id;
        }

        _wrapper.SetActive(true);
    }

    public void Hide()
    {
        _wrapper.SetActive(false);

        foreach(CardView card in _cards)
        {
            Destroy(card.gameObject);
        }

        _cards.Clear();
    }

    public void PressSkip()
    {
        _action.Invoke(null);
        Hide();
    }
}
