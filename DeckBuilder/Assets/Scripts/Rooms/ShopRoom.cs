using UnityEngine;

public class ShopRoom : Room
{
    public override RoomType RoomType => RoomType.SHOP;

    public ShopRoom(int level, int row, int seed) : base(level, row, seed) { }

    public override void Enter()
    {
        MapSystem.Instance.EnterShop();
    }

    public override void SetUp() { }
}
