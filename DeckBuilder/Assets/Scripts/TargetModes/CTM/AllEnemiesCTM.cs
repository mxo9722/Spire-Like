using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext targetModeContext)
    {
        return new(BoardSystem.Instance.BoardView.GetAllEnemies());
    }
}
