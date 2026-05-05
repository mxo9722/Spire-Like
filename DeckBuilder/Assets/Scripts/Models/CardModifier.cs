using System;
using UnityEngine;

[System.Serializable]
public abstract class CardModifier : ICloneable
{
    protected virtual void ApplyVisualEffects(CardView card) {}

    public abstract bool CanApply(Card card);
    public abstract bool TryToCombine(CardModifier cardModifier);
    public abstract object Clone();
}
