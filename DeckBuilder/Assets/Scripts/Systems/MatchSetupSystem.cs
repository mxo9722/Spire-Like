using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : Singleton<MatchSetUpSystem>
{
    [SerializeField] private HeroData _heroData1;
    [SerializeField] private HeroData _heroData2;
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
            Hero runHero1 = RunSystem.Instance.Hero1;
            Hero runHero2 = RunSystem.Instance.Hero2;

            if (runHero1 != null)
                _heroData1 = runHero1.Data;
            if (runHero2 != null)
                _heroData2 = runHero1.Data;


            HeroSystem.Instance.Setup(runHero1, runHero2);

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
        ShuffleGA shuffleGA = new();
        ActionSystem.Instance.AddReaction(shuffleGA);

        DrawCardsGA drawCardsGA = new(7);
        ActionSystem.Instance.AddReaction(drawCardsGA);
        yield return null;
    }
}
