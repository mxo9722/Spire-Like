using UnityEngine;

public class CombatantViewCreator : Singleton<CombatantViewCreator>
{
    [SerializeField] private HeroView _heroViewPrefab;
    [SerializeField] private EnemyView _enemyViewPrefab;

    public HeroView CreateHeroView(HeroData heroData, Vector3 position, Quaternion rotation)
    {
        HeroView heroView = Instantiate(_heroViewPrefab, position, rotation);
        heroView.Setup(heroData);
        return heroView;
    }
    
    public EnemyView CreateEnemyView(EnemyData enemyData, Vector3 position, Quaternion rotation)
    {
        EnemyView enemyView = Instantiate(_enemyViewPrefab, position, rotation);
        enemyView.Setup(enemyData);
        return enemyView;
    }
}
