using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    public HeroView[] HeroViews { get; private set; }

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

    public void Setup(Hero heroData1, Hero heroData2)
    {
        HeroViews = new HeroView[2];
        HeroViews[0] = BoardSystem.Instance.BoardView.CreateHero(heroData1, 0);
        HeroViews[1] = BoardSystem.Instance.BoardView.CreateHero(heroData2, 2);
    }

    private IEnumerator AfterPlayerTurnPerformer(AfterPlayerTurnGA afterPlayerTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new(true);
        ActionSystem.Instance.AddReaction(discardAllCardsGA);

        foreach(HeroView heroView in HeroViews)
            StatusEffectSystem.Instance.PrePostModifyStatusEffect(heroView, ReactionTiming.POST);
        yield return null;
        
        NPCTurnGA enemyTurnGA;

        enemyTurnGA = new NPCTurnGA(BoardSystem.Instance.GetAllEnemies());
        ActionSystem.Instance.AddReaction(enemyTurnGA);

        BeforePlayerTurnGA beforePlayerTurnGA = new();
        ActionSystem.Instance.AddReaction(beforePlayerTurnGA);
    }

    private IEnumerator BeforePlayerTurnPerformer(BeforePlayerTurnGA beforePlayerTurnGA)
    {
        foreach (HeroView heroView in HeroViews)
            StatusEffectSystem.Instance.PrePostModifyStatusEffect(heroView, ReactionTiming.PRE);

        //RedistributeEnemiesGA redistributeEnemiesGA = new RedistributeEnemiesGA();
        //ActionSystem.Instance.AddReaction(redistributeEnemiesGA);

        DrawCardsGA drawCardsGA = new(7);
        ActionSystem.Instance.AddReaction(drawCardsGA);

        yield return null;
    }
}
