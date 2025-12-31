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
        ActionSystem.AttachPerformer<BeforePlayerTurnGA>(BeforePlayerTurnPerformer);
        ActionSystem.AttachPerformer<AfterPlayerTurnGA>(AfterPlayerTurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<BeforePlayerTurnGA>();
        ActionSystem.DetachPerformer<AfterPlayerTurnGA>();
    }

    public void Setup(HeroData heroData)
    {
        HeroView = BoardSystem.Instance.BoardView.CreateHero(heroData, 1);
    }

    private IEnumerator AfterPlayerTurnPerformer(AfterPlayerTurnGA afterPlayerTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new(true);
        ActionSystem.Instance.AddReaction(discardAllCardsGA);

        StatusEffectSystem.Instance.PrePostModifyStatusEffect(HeroView, ReactionTiming.POST);
        yield return null;
        
        NPCTurnGA enemyTurnGA;
        List<NPCView> sideKicks = BoardSystem.Instance.GetAllSideKicks();

        if (sideKicks.Count > 0)
        {
            enemyTurnGA = new NPCTurnGA(sideKicks);
            ActionSystem.Instance.AddReaction(enemyTurnGA);
        }

        enemyTurnGA = new NPCTurnGA(BoardSystem.Instance.GetAllEnemies());
        ActionSystem.Instance.AddReaction(enemyTurnGA);

        BeforePlayerTurnGA beforePlayerTurnGA = new();
        ActionSystem.Instance.AddReaction(beforePlayerTurnGA);
    }

    private IEnumerator BeforePlayerTurnPerformer(BeforePlayerTurnGA beforePlayerTurnGA)
    {
        StatusEffectSystem.Instance.PrePostModifyStatusEffect(HeroView, ReactionTiming.PRE);

        //RedistributeEnemiesGA redistributeEnemiesGA = new RedistributeEnemiesGA();
        //ActionSystem.Instance.AddReaction(redistributeEnemiesGA);

        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);

        yield return null;
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
