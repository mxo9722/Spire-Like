using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewCardRandomOwnerCaTM : CardTargetMode
{
    [SerializeField] private List<CardData> _cards;
    [SerializeField] private bool _randomizeEachCard = false;

    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> cards = new();

        HeroData[] possibleDatas = HeroSystem.Instance.HeroViews.Select(h => h.HeroData).ToArray();

        HeroData owner = RNG.SelectRandom(possibleDatas);

        foreach (CardData data in _cards)
        {
            Card card = new(data, owner);

            cards.Add(card);

            if(_randomizeEachCard)
                owner = RNG.SelectRandom(possibleDatas);
        }

        return cards;
    }
}
