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
        ActionSystem.AttachPerformer<MoveUnitsGA>(MoveCombatantsPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<RedistributeEnemiesGA>();
        ActionSystem.DetachPerformer<CompressBoardGA>();
        ActionSystem.DetachPerformer<RemoveLaneGA>();
        ActionSystem.DetachPerformer<MoveUnitsGA>();
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

    private IEnumerator MoveCombatantsPerformer(MoveUnitsGA moveEnemyGA)
    {
        yield return BoardView.MoveCombatants(moveEnemyGA, 0.5f);
    }

    public LaneView GetCurrentLaneView(CombatantView combatantView)
    {
        if (combatantView is HeroView heroView)
            return BoardView.GetCurrentLaneView(heroView);
        else if (combatantView is NPCView enemyView)
            return BoardView.GetCurrentLaneView(enemyView);

        throw new System.Exception();
    }
    
    public LaneView GetCurrentLaneView(HeroView heroView) => BoardView.GetCurrentLaneView(heroView);    
    public LaneView GetCurrentLaneView(NPCView enemyView) => BoardView.GetCurrentLaneView(enemyView);
    public List<NPCView> GetAllEnemies() => BoardView.GetAllEnemies();
    public List<NPCView> GetAllSideKicks() => BoardView.GetAllSideKicks();
    public List<CombatantView> GetAllCombatants() => BoardView.GetAllCombatants();
    public List<CombatantView> GetAllFoes(CombatantView caster) => BoardView.GetAllFoes(caster);
    public List<LaneView> GetAllLanes() => BoardView.GetAllLanes();

    public LaneView GetLaneFromDirection(LaneView laneView, MovementDirection direction)
    {
        var lanes = BoardView.GetAllLanes();
        int index = lanes.IndexOf(laneView);

        switch (direction)
        {
            case MovementDirection.UP:
                index--;
                break;
            case MovementDirection.DOWN:
                index++;
                break;
        }

        index = Mathf.Clamp(index, 0, lanes.Count - 1);

        return lanes[index];
    }

    public List<T> GetAllViews<T>()
    {
        if (typeof(T).ToString() == typeof(LaneView).ToString())
            return GetAllLanes() as List<T>;
        if (typeof(T).ToString() == typeof(CombatantView).ToString())
            return GetAllCombatants() as List<T>;

        return new();
        
    }
}