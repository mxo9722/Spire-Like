using System.Collections.Generic;
using UnityEngine;

public class MoveTowardTargetEffect : Effect
{
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        if (targets.Count == 0)
            return null;

        GameAction gameAction = null;

        CombatantView targetView = targets[0];
        LaneView targetLaneView = BoardSystem.Instance.GetCurrentLaneView(targetView);
        LaneView originalLaneView = BoardSystem.Instance.GetCurrentLaneView(caster);

        if (targetLaneView == originalLaneView)
            return null;


        if(caster is HeroView heroView)
        {
            gameAction = new MoveHeroGA(targetLaneView, heroView);
        }
        else if(caster is EnemyView enemyView)
        {
            gameAction = new MoveEnemyGA(targetLaneView, enemyView);
        }
        

        return gameAction;
    }
}
