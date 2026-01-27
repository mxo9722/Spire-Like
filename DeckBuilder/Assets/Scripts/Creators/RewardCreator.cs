using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardCreator : Singleton<RewardCreator>
{
    [field: SerializeField] public List<CardData> ColorlessCards { get; private set; }
    [field: SerializeField] public List<PerkData> Perks { get; private set; }

    [SerializeField] private int _creditMin = 10;
    [SerializeField] private int _creditMax = 20;


    public CardReward CreateCardPick()
    {
        List<Card> pack = new();

        int cardCount = 3;

        List<Card> allOptions = new();

        allOptions.AddRange(RunSystem.Instance.Hero1.ClassCards.Select(c => new Card(c, RunSystem.Instance.Hero1.Data))); 
        allOptions.AddRange(RunSystem.Instance.Hero2.ClassCards.Select(c => new Card(c, RunSystem.Instance.Hero2.Data)));


        for (int x = 0; x < cardCount; x++)
        {
            Card card = allOptions[RNG.Random.Next(0, allOptions.Count)];
            pack.Add(card);
            allOptions.Remove(card);
        }

        CardReward reward = new();
        reward.SetCards(pack);
        return reward;
    }

    public RenownReward CreateMoney()
    {
        return CreateMoney(_creditMin,_creditMax);
    }

    public RenownReward CreateMoney(int min, int max)
    {
        RenownReward reward = new();
        reward.Setcredits(RNG.Random.Next(min, max + 1));
        return reward;
    }

    public PerkReward CreatePerk()
    {
        List<PerkData> allOptions = new(); 
        allOptions.AddRange(Perks);
        allOptions.AddRange(RunSystem.Instance.Hero1.Perks);
        allOptions.AddRange(RunSystem.Instance.Hero2.Perks);

        if (allOptions.Count == 0)
        {
            return null;
        }

        allOptions.RemoveAll(r => RunSystem.Instance.UsedPerks.Contains(r));

        if(allOptions.Count == 0)
        {
            RunSystem.Instance.UsedPerks.Clear();
            return CreatePerk();
        }

        PerkReward reward = new();
        reward.SetPerk(allOptions[RNG.Random.Next(0, allOptions.Count)]);
        return reward;
    }

    public Card GetRandomCard()
    {
        return GetRandomCards(1)[0];
    }

    public Card[] GetRandomCards(int count)
    {
        List<Card> allCardOptions = new();
        allCardOptions.AddRange(RunSystem.Instance.Hero1.ClassCards.Select(c => new Card(c, RunSystem.Instance.Hero1.Data)));
        allCardOptions.AddRange(RunSystem.Instance.Hero2.ClassCards.Select(c => new Card(c, RunSystem.Instance.Hero2.Data)));

        //allCardOptions.AddRange(ColorlessCards);

        Card[] results = new Card[count];

        for(int i = 0; i < count; i++)
        {
            int randomIndex = RNG.Random.Next(allCardOptions.Count);
            results[i] = allCardOptions[randomIndex];
            allCardOptions.RemoveAt(randomIndex);
        }

        return results;
    }
}
