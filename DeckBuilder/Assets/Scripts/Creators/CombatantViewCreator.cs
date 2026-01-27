using UnityEngine;

public class CombatantViewCreator : Singleton<CombatantViewCreator>
{
    [SerializeField] private HeroView _heroViewPrefab;
    [SerializeField] private NPCView _enemyViewPrefab;
    [SerializeField] private NPCView _sideKickViewPrefab;

    public HeroView CreateHeroView(Hero heroData, SlotView slot)
    {
        HeroView heroView = Instantiate(_heroViewPrefab, slot.transform.position, slot.transform.rotation);
        heroView.Setup(heroData, slot);
        return heroView;
    }
    
    public NPCView CreateEnemyView(NPCData enemyData, SlotView slot)
    {
        NPCView enemyView = Instantiate(_enemyViewPrefab, slot.transform.position, slot.transform.rotation);
        enemyView.Setup(enemyData, slot);
        return enemyView;
    }

    public NPCView CreateSideKickView(NPCData npcData, SlotView slot)
    {
        NPCView sidekickView = Instantiate(_sideKickViewPrefab, slot.transform.position, slot.transform.rotation);
        sidekickView.Setup(npcData, slot, false);

        EnemySystem.Instance.UpdateEnemiesBehaviorUI();

        return sidekickView;
    }
}
