using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    
    private readonly List<CardView> _cards = new();

    public IEnumerator AddCard(CardView card, int index = -1)
    {
        if(index == -1)
            yield return InsertCard(card, _cards.Count);
        else
            yield return InsertCard(card, Math.Min(_cards.Count, index));
    }

    public IEnumerator InsertCard(CardView card,int position)
    {
        _cards.Insert(position, card);
        yield return UpdateCardPositions(0.15f);
        card.SetTreatAsButton(false);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;
        _cards.Remove(cardView);

        StartCoroutine(UpdateCardPositions(0.15f));
        return cardView;
    }

    public void UpdateCardHoverView()
    {
        foreach(CardView card in _cards)
        {
            card.UpdateHoverView();
        }
    }

    public IEnumerator ApplyCardThrob(Card card, float duration)
    {
        CardView cardView = _cards.Find(c => c.Card == card);

        if(cardView == null)
        {
            Vector3 scale = cardView.transform.localScale;
            yield return cardView.transform.DOScale(scale * 1.25f, duration/2.0f).WaitForCompletion();
            yield return cardView.transform.DOScale(scale, duration/2.0f).WaitForCompletion();
        }
    }

    private CardView GetCardView(Card card)
    {
        return _cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }

    private IEnumerator UpdateCardPositions(float duration)
    {
        if (_cards.Count == 0) yield break;

        float cardSpacing = Mathf.Lerp( 0.2f, 0.1f, _cards.Count / (float)CardSystem.MAX_HAND_SIZE);
        float firstCardPos = 0.5f - (_cards.Count - 1) * cardSpacing / 2;
        Spline spline = _splineContainer.Spline;

        for(int i = 0; i < _cards.Count; i++)
        {
            float p = firstCardPos + i * cardSpacing;
            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            Vector3 newPos = splinePos + transform.position + 0.01f * i * Vector3.back;

            _cards[i].transform.DOMove(newPos, duration);
            _cards[i].transform.DORotate(rotation.eulerAngles,duration);
            _cards[i].SetBasePos(newPos, rotation);
        }
        yield return new WaitForSeconds(duration);
    }

    public void UpdateCardViews()
    {
        foreach(CardView card in _cards)
        {
            card.UpdateDynamicDescription(new(card.Card.GetOwnerView(), playedCard: card.Card));

            card.UpdateGlow();
        }
    }
}
