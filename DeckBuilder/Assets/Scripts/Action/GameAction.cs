using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new();
    public List<GameAction> PreformReactions { get; private set; } = new();
    public List<GameAction> PostReactions { get; private set; } = new();

    public virtual bool PerformAfterGameOver { get; } = false;

    public virtual bool TryCombine(GameAction other) { return false; }
}

public abstract class CombinableGameAction<T> : GameAction where T : CombinableGameAction<T>
{

    public override bool TryCombine(GameAction other)
    {
        if (other is T t)
        {
            Combine(t);
            return true;
        }

        return false;
    }

    public abstract void Combine(T other);
}
