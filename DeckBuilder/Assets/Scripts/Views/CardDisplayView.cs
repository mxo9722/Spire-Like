using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CardDisplayView : Singleton<CardDisplayView>
{

    [SerializeField] private SplineContainer _splineContainer;

    public IEnumerator DisplayCards(List<CardView> cards)
    {
        Tween tween = null;

        for (int i = 0; i < cards.Count; i++)
        {
            float p = (i + 1.00f) / (cards.Count + 1.00f);

            Vector3 position = _splineContainer.Spline.EvaluatePosition(p);

            CardView cardView = cards[i];

            yield return new WaitForSeconds(0.25f);

            tween = cardView.transform.DOMove(position, 0.5f);
        }

        yield return tween.WaitForCompletion();
    }

}
