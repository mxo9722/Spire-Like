using UnityEngine;

public class SpendDashesGA : GameAction
{
    public int Amount { get; private set; }

    public SpendDashesGA(int amount = 1)
    {
        Amount = amount;
    }
}
