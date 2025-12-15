using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomEnemiesCTM : CombatantTargetMode
{
    [SerializeField, Min(1)] private int _enemyCount = 1;

    public override bool IsRandom => true;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        return GetTargets(context, RNG.Random);
    }

    public override List<CombatantView> GetTargetsTrivial(EffectContext context)
    {
        return GetTargets(context, RNG.TrivialRandom);
    }

    private List<CombatantView> GetTargets(EffectContext targetModeContext, System.Random random)
    {
        List<EnemyView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();

        if (enemies.Count == 0)
            return new();
        else if (enemies.Count <= _enemyCount)
            return new(enemies);

        List<CombatantView> targets = new();

        for (int i = 0; i < _enemyCount; i++)
        {
            int index = RNG.Random.Next(enemies.Count);
            EnemyView target = enemies[index];
            enemies.RemoveAt(index);
            targets.Add(target);
        }

        return targets;
    }

    public override List<CombatantView> AllPossibleTargets(EffectContext context, Card card = null)
    {
        return new(BoardSystem.Instance.BoardView.GetAllEnemies());
    }
}
