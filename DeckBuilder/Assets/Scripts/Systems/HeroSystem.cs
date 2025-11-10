using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field: SerializeField] public HeroView HeroView { get; private set; }

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
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        int burnStack = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);

        if(burnStack > 0)
        {
            ApplyBurnGA applyBurnGA = new(burnStack, HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGA);
        }

        //RedistributeEnemiesGA redistributeEnemiesGA = new RedistributeEnemiesGA();
        //ActionSystem.Instance.AddReaction(redistributeEnemiesGA);

        CompressBoardGA compressBoardGA = new();
        ActionSystem.Instance.AddReaction(compressBoardGA);

        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }
}
