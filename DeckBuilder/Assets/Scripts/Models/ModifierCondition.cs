using System;
using UnityEngine;

[Serializable]
public abstract class ModifierCondition
{
    [SerializeField] protected ConditionalModifierSystem.ModifierTiming _timing = ConditionalModifierSystem.ModifierTiming.MID;

    public abstract void Subscribe(ConditionalModifierSystem.ModifierDelegate action, object subscriber);
    public abstract void Unsubscribe(object subscriber);
    public abstract bool ConditionIsMet(ModifierKey key);
}
