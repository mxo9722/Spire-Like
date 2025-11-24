using System.Collections.Generic;
using UnityEngine;

public class MoveTowardCombatantEffect : CombatantTargetEffect
{

    protected override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets)
    {
        if ((combatantTargets == null || combatantTargets.Count == 0))
            return null;

        GameAction gameAction = null;

        CombatantView targetView = combatantTargets[0];
        LaneView targetLaneView = BoardSystem.Instance.GetCurrentLaneView(targetView);

        if (caster is HeroView heroView)
        {
            gameAction = new MoveHeroGA(targetLaneView, heroView);
        }
        else if (caster is EnemyView enemyView)
        {
            gameAction = new MoveEnemyGA(targetLaneView, enemyView);
        }

        return gameAction;
    }
}
