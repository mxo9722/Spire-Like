using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        List<EnemyView> enemies = EnemySystem.Instance.Enemies;

        if (enemies.Count == 0)
            return new();

        return new(){ enemies[RNG.Random.Next(enemies.Count)] };
    }
}
