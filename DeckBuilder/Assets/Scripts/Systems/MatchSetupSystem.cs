using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : Singleton<MatchSetUpSystem>
{
    [SerializeField] private HeroData _heroData;
    [SerializeField] private PerkData _perkData;

    [field: SerializeField] public CombatRoom Room { get; private set; }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<CombatStartGA>(CombatStartPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<CombatStartGA>();
    }

    private void Start()
    {
        if (RunSystem.Instance.GetRoom() is CombatRoom combatRoom)
            Room = combatRoom;

        if (Room.IsCompleted)
        {
            RewardSystem.Instance.DisplayRewards(Room.Rewards, MatchEndSystem.Instance.ReturnToMap);
        }
        else
        {
            HeroData runHero = RunSystem.Instance.GetHeroData();

            if (runHero != null)
                _heroData = runHero;

            HeroSystem.Instance.Setup(_heroData);

            EnemySystem.Instance.Setup(Room.TopRow, 0);
            EnemySystem.Instance.Setup(Room.MiddleRow, 1);
            EnemySystem.Instance.Setup(Room.BottomRow, 2);

            CardSystem.Instance.SetUp(RunSystem.Instance.RunData.Deck);

            ManaSystem.Instance.UpdateManaText();

            ActionSystem.Instance.Perform(new CombatStartGA());
        }
    }

    private IEnumerator CombatStartPerformer(CombatStartGA combatStartGA)
    {
        var combatants = BoardSystem.Instance.GetAllCombatants();

        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
        yield return null;
    }
}
