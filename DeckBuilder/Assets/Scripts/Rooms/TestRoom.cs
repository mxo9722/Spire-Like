using UnityEngine;

public class TestRoom : Room
{
    private RoomType _roomType;
    public override RoomType RoomType => _roomType;

    public TestRoom(int level, int row, int seed, RoomType type) : base(level,row,seed)
    {
        _roomType = type;
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void SetUp() { }
}
