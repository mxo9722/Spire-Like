using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    public HeroView HeroView { get; private set; }

    private void OnEnable()
    {

        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void Setup(HeroData heroData)
    {
        HeroView = BoardSystem.Instance.BoardView.CreateHero(heroData, 1);
    }

    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new(true);
        ActionSystem.Instance.AddReaction(discardAllCardsGA);

        StatusEffectSystem.Instance.PrePostModifyStatusEffect(HeroView, ReactionTiming.POST);

        List<EnemyView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();

        StatusEffectSystem.Instance.PrePostModifyStatusEffect(enemies.Select(e => (CombatantView)e).ToList(), ReactionTiming.PRE);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        List<EnemyView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();

        StatusEffectSystem.Instance.PrePostModifyStatusEffect(enemies.Select(e => (CombatantView)e).ToList(), ReactionTiming.POST);

        StatusEffectSystem.Instance.PrePostModifyStatusEffect(HeroView, ReactionTiming.PRE);

        int burnStack = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);

        if(burnStack > 0)
        {
            ApplyBurnGA applyBurnGA = new(HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGA);
        }

        //RedistributeEnemiesGA redistributeEnemiesGA = new RedistributeEnemiesGA();
        //ActionSystem.Instance.AddReaction(redistributeEnemiesGA);

        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }

    public int GetHealth()
    {
        if(HeroView == null)
        {
            return RunSystem.Instance.CurrentHealth;
        }

        return HeroView.CurrentHealth;
    }
    
    public int GetMaxHealth()
    {
        if(HeroView == null)
        {
            return RunSystem.Instance.MaxHealth;
        }

        return HeroView.MaxHealth;
    }
}
