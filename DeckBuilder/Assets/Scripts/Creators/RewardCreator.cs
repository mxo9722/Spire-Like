using System.Collections.Generic;
using UnityEngine;

public class RewardCreator : Singleton<RewardCreator>
{
    [field: SerializeField] public List<CardData> ColorlessCards { get; private set; }
    [field: SerializeField] public List<PerkData> Perks { get; private set; }

    [SerializeField] private int _creditMin = 10;
    [SerializeField] private int _creditMax = 20;


    public CardReward CreateCardPick()
    {
        List<CardData> pack = new();

        int cardCount = 3;

        List<CardData> allOptions = new(RunSystem.Instance.GetHeroData().GetClassCards());

        for(int x = 0; x < cardCount; x++)
        {
            var card = allOptions[RNG.Random.Next(0, allOptions.Count)];
            pack.Add(card);
            allOptions.Remove(card);
        }

        CardReward reward = new();
        reward.SetCards(pack);
        return reward;
    }

    public KarmaReward CreateMoney()
    {
        return CreateMoney(_creditMin,_creditMax);
    }

    public KarmaReward CreateMoney(int min, int max)
    {
        KarmaReward reward = new();
        reward.Setcredits(RNG.Random.Next(min, max + 1));
        return reward;
    }

    public PerkReward CreatePerk()
    {
        List<PerkData> allOptions = RunSystem.Instance.GetHeroData().GetClassPerks();
        allOptions.AddRange(Perks);

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
}
