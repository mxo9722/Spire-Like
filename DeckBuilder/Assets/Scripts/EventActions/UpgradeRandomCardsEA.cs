using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeRandomCardsEA : EventAction
{

    [SerializeField] private int cardCount = 1;

    public override IEnumerator Invoke()
    {
        List<Card> cards = RunSystem.Instance.Deck.Select(cd => new Card(cd)).ToList();
        cards.RemoveAll(c => c.Upgrade == null);
        cards.Shuffle();

        List<CardView> cardViews = new();
        
        for(int i = 0; i < cardCount; i++)
        {
            CardView cardView = CardViewCreator.Instance.CreateCardView(cards[i], TopBarUI.Instance.DeckUIPos, Quaternion.identity, true);
            cardView.SetGlow(Color.clear);
            cardViews.Add(cardView);
        }

        yield return CardDisplayView.Instance.DisplayCards(cardViews);

        yield return new WaitForSeconds(3);

        Tween tween = null;

        foreach (CardView cardView in cardViews)
        {
            tween = cardView.transform.DORotate(new(0, 180, 0), 0.25f);
            tween.onComplete += () => 
            {
                cardView.transform.DORotate(Vector3.zero, 0.25f);
                RunSystem.Instance.UpgradeCard(cardView.Card.data);
                cardView.Setup(new(cardView.Card.Upgrade),true);
                cardView.SetGlow(Color.clear);
            };
            yield return new WaitForSeconds(0.15f);
        }

        yield return tween.WaitForCompletion();

        yield return new WaitForSeconds(3);

        foreach (CardView cardView in cardViews)
        {
            tween = cardView.transform.DOMove(TopBarUI.Instance.DeckUIPos, 0.25f);
            cardView.transform.DOScale(Vector3.zero, 0.25f);
            yield return new WaitForSeconds(0.15f);
        }

        yield return tween.WaitForCompletion();
        foreach (CardView cardView in cardViews)
        {
            cardView.transform.DOKill();
            Object.Destroy(cardView.gameObject);
        }
    }
}
