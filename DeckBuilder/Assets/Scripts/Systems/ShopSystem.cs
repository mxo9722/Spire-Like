using UnityEngine;

public class ShopSystem : Singleton<ShopSystem>
{

    [SerializeField] private ShopView _shopView;

    private void Start()
    {
        _shopView.SetUp();
    }
}
