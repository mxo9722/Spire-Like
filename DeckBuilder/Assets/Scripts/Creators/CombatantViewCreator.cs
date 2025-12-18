using UnityEngine;

public class CombatantViewCreator : Singleton<CombatantViewCreator>
{
    [SerializeField] private HeroView _heroViewPrefab;
    [SerializeField] private EnemyView _enemyViewPrefab;

    public HeroView CreateHeroView(HeroData heroData, SlotView slot)
    {
        HeroView heroView = Instantiate(_heroViewPrefab, slot.transform.position, slot.transform.rotation);
        heroView.Setup(heroData, slot);
        return heroView;
    }
    
    public EnemyView CreateEnemyView(EnemyData enemyData, SlotView slot)
    {
        EnemyView enemyView = Instantiate(_enemyViewPrefab, slot.transform.position, slot.transform.rotation);
        enemyView.Setup(enemyData, slot);
        return enemyView;
    }
}
