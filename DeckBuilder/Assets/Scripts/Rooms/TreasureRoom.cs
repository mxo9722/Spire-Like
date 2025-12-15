using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : Room, IHaveRewards
{
    public override RoomType RoomType => RoomType.TREASURE;
    [field: SerializeReference, SR] public List<SetReward> Rewards { get; private set; } = new();

    public TreasureRoom(int level, int row, int seed) : base(level, row, seed) { }

    public void AddReward(SetReward reward) => Rewards.Add(reward);

    public override void Enter()
    {
        RewardSystem.Instance.DisplayRewards(Rewards, CompleteRoom);
    }

    public override void SetUp() 
    {
        Rewards.Add(RewardCreator.Instance.CreatePerk());
        Rewards.Add(RewardCreator.Instance.CreateMoney(23, 27));
    }

    public void CompleteRoom()
    {
        SetCompleted();
        MapSystem.Instance.RefreshMap(0.5f);
        RunSystem.Instance.SaveRun();
    }

    public void RemoveReward(SetReward reward) => Rewards.Remove(reward);
}
