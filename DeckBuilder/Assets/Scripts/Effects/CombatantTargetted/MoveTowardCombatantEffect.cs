using System.Collections.Generic;
using UnityEngine;

public class MoveTowardCombatantEffect : CombatantTargetEffect
{

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        if ((combatantTargets == null || combatantTargets.Count == 0))
            return null;

        GameAction gameAction = null;

        CombatantView targetView = combatantTargets[0];
        LaneView targetLaneView = BoardSystem.Instance.GetCurrentLaneView(targetView);

        if (context.Caster is HeroView heroView)
        {
            gameAction = new MoveHeroGA(targetLaneView, heroView);
        }
        else if (context.Caster is EnemyView enemyView)
        {
            gameAction = new MoveEnemyGA(targetLaneView, enemyView);
        }

        return gameAction;
    }
}
