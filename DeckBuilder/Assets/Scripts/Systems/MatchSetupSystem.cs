using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : MonoBehaviour
{
    [SerializeField] private HeroData _heroData;
    [SerializeField] private PerkData _perkData;
    [SerializeField] private List<EnemyData> _topEnemyLane;
    [SerializeField] private List<EnemyData> _middleEnemyLane;
    [SerializeField] private List<EnemyData> _bottomEnemyLane;

    private void Start()
    {
        HeroSystem.Instance.Setup(_heroData);
        EnemySystem.Instance.Setup(_topEnemyLane,0);
        EnemySystem.Instance.Setup(_middleEnemyLane,1);
        EnemySystem.Instance.Setup(_bottomEnemyLane,2);
        CardSystem.Instance.SetUp(_heroData.Deck);
        PerkSystem.Instance.AddPerk(new Perk(_perkData));
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
