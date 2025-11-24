using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomEnemiesCTM : CombatantTargetMode
{
    [SerializeField, Min(1)] private int _enemyCount = 1;

    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        List<EnemyView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();

        if (enemies.Count == 0)
            return new();
        else if (enemies.Count <= _enemyCount)
            return new(enemies);

        List<CombatantView> targets = new();

        for(int i = 0; i < _enemyCount; i++)
        {
            int index = RNG.Random.Next(enemies.Count);
            EnemyView target = enemies[index];
            enemies.RemoveAt(index);
            targets.Add(target);
        }

        return targets;
    }
}
