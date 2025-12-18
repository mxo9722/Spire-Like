using UnityEngine;

public class SafeHouseRoom : Room
{
    public override RoomType RoomType => RoomType.SAFE_HOUSE;

    public SafeHouseRoom(int level, int row, int seed) : base(level, row, seed) { }

    public override void Enter()
    {
        MapSystem.Instance.EnterEvent();
    }

    public override void SetUp() { }
}
