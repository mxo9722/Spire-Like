using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public class RemoveCardFromDeckEA : EventAction
{
    [SerializeField, Min(1)] private int _amount = 1;

    public override IEnumerator Invoke(EffectContext context)
    {
        List<Card> deck = new(RunSystem.Instance.Deck);
        CardCollectionSystem.Instance.SelectionDisplay(deck, _amount, false);
        yield return new WaitUntil(() => !CardCollectionSystem.Instance.WaitingForSelection);
        List<Card> selection = CardCollectionSystem.Instance.GetCardSelections();

        foreach (Card card in selection)
            RunSystem.Instance.RemoveCard(card);
    }

}
