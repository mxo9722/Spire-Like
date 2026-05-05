using SerializeReferenceEditor;
using System;
using UnityEngine;

[Serializable]
public class ConditionalModifierPair
{
    [field: SerializeReference, SR] public ModifierCondition Condition { get; private set; }
    [field: SerializeReference, SR] public Modifier Modifier { get; private set; }

    public void Subscribe(object subscriber)
    {
        Condition.Subscribe(TestCondition, subscriber);
    }
    
    public void Unsubscribe(object subscriber)
    {
        Condition.Unsubscribe(subscriber);
    }

    public int TestCondition(int oValue, ModifierKey key)
    {
        if (Condition.ConditionIsMet(key))
            return Modifier.GetValue(oValue, key);

        return oValue;
    }
}
