using UnityEngine;

public class SpotlightCardGA : GameAction
{
    [field: SerializeField] public CardView Target { get; private set; }

    public SpotlightCardGA(CardView target)
    {
        Target = target;
    }
}
