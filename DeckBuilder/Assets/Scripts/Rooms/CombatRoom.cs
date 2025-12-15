using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CombatRoom : Room, IHaveRewards
{
    [field: SerializeField] public List<EnemyData> TopRow { get; private set; } = new();
    [field: SerializeField] public List<EnemyData> MiddleRow { get; private set; } = new();
    [field: SerializeField] public List<EnemyData> BottomRow { get; private set; } = new();
    [field: SerializeReference, SR] public List<SetReward> Rewards { get; private set; } = new();

    public int FightIndex { get; private set; } = -1;

    public override RoomType RoomType => RoomType.FIGHT;

    public CombatRoom(int level, int row, int seed = 0) : base(level, row, seed)
    {
        TopRow = new();
        MiddleRow = new();
        BottomRow = new();

        Rewards.Add(RewardCreator.Instance.CreateCardPick());
        Rewards.Add(RewardCreator.Instance.CreateMoney());
    }

    public override void SetUp()
    {
        List<int> skipList = OriginRooms.Where(n => n.RoomType == RoomType).Select(n => ((CombatRoom)n).FightIndex).ToList();
        FightIndex = MapSystem.Instance.GetRoomIndex(skipList, RoomType);
        MapSystem.Instance.SetUpCombatRoom(this, FightIndex);
    }

    public void SetFightIndex(int index)
    {
        FightIndex = index;
    }

    public override void Enter()
    {
        MapSystem.Instance.EnterCombat();
    }

    public void RemoveReward(SetReward reward) => Rewards.Remove(reward);
}
