using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return new(BoardSystem.Instance.BoardView.GetAllEnemies());
    }
}
