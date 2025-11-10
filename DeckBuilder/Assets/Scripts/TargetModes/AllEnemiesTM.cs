using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesTM : TargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return new(BoardSystem.Instance.BoardView.GetAllEnemies());
    }
}
