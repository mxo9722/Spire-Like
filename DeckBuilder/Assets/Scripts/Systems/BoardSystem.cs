using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSystem : Singleton<BoardSystem>
{
    [field: SerializeField] public BoardView BoardView { get; private set; }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<RedistributeEnemiesGA>(RedistributeEnemiesPerformer);
        ActionSystem.AttachPerformer<CompressBoardGA>(CompressBoardPerformer);
        ActionSystem.AttachPerformer<RemoveLaneGA>(RemoveLanePerformer);
        ActionSystem.AttachPerformer<MoveEnemyGA>(MoveEnemyPerformer);
        ActionSystem.AttachPerformer<MoveHeroGA>(MoveHeroPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<RedistributeEnemiesGA>();
        ActionSystem.DetachPerformer<CompressBoardGA>();
        ActionSystem.DetachPerformer<RemoveLaneGA>();
        ActionSystem.DetachPerformer<MoveEnemyGA>();
        ActionSystem.DetachPerformer<MoveHeroGA>();
    }

    private IEnumerator RedistributeEnemiesPerformer(RedistributeEnemiesGA updateBoardGA)
    {
        yield return BoardView.RedistributeEnemies();
    }
    
    private IEnumerator CompressBoardPerformer(CompressBoardGA compressBoardGA)
    {
        yield return BoardView.CompressBoard();
    }
    
    private IEnumerator RemoveLanePerformer(RemoveLaneGA removeLaneGA)
    {
        yield return BoardView.RemoveLane(removeLaneGA.LaneView, 0.5f);
    }

    private IEnumerator MoveEnemyPerformer(MoveEnemyGA moveEnemyGA)
    {
        yield return BoardView.MoveEnemy(moveEnemyGA, 0.5f);
    }
    
    private IEnumerator MoveHeroPerformer(MoveHeroGA moveHeroGA)
    {
        yield return BoardView.MoveHero(moveHeroGA, 0.5f);
    }

    public LaneView GetCurrentLaneView(CombatantView combatantView)
    {
        if (combatantView is HeroView heroView)
            return BoardView.GetCurrentLaneView(heroView);
        else if (combatantView is EnemyView enemyView)
            return BoardView.GetCurrentLaneView(enemyView);

        throw new System.Exception();
    }
    
    public LaneView GetCurrentLaneView(HeroView heroView) => BoardView.GetCurrentLaneView(heroView);    
    public LaneView GetCurrentLaneView(EnemyView enemyView) => BoardView.GetCurrentLaneView(enemyView);
    public List<EnemyView> GetAllEnemies() => BoardView.GetAllEnemies();
    public List<CombatantView> GetAllCombatants() => BoardView.GetAllCombatants();
    public List<LaneView> GetAllLanes() => BoardView.GetAllLanes();

    public List<T> GetAllViews<T>()
    {
        if (typeof(T).ToString() == typeof(LaneView).ToString())
            return GetAllLanes() as List<T>;
        if (typeof(T).ToString() == typeof(CombatantView).ToString())
            return GetAllCombatants() as List<T>;

        return new();
        
    }
}
