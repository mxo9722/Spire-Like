using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        List<EnemyView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();

        if (enemies.Count == 0)
            return new();

        return new(){ enemies[RNG.Random.Next(enemies.Count)] };
    }
}
