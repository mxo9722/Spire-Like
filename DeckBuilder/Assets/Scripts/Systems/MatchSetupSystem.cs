using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : MonoBehaviour
{
    [SerializeField] private HeroData _heroData;
    [SerializeField] private PerkData _perkData;
    [SerializeField] private List<EnemyData> _enemyDatas;

    private void Start()
    {
        HeroSystem.Instance.Setup(_heroData);
        EnemySystem.Instance.Setup(_enemyDatas);
        CardSystem.Instance.SetUp(_heroData.Deck);
        PerkSystem.Instance.AddPerk(new Perk(_perkData));
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
