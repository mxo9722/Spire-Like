using UnityEngine;

public class BossRoom : CombatRoom
{

    public override RoomType RoomType => RoomType.ELITE;

    public BossRoom(int level, int row, int seed) : base(level, row, seed) 
    {
        Rewards.Add(RewardCreator.Instance.CreatePerk());
    }
}
