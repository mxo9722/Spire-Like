using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class ManaCostMC : ModifierCondition
{

    [SerializeReference, SR] private List<CardFilter> _cardFilters;

    public override void Subscribe(ConditionalModifierSystem.ModifierDelegate action, object subscriber)
    {
        ConditionalModifierSystem.Subscribe<ManaModKey>(action, subscriber, _timing);
    }

    public override void Unsubscribe(object subscriber)
    {
        ConditionalModifierSystem.Unsubscribe<ManaModKey>(subscriber, _timing);
    }

    public override bool ConditionIsMet(ModifierKey key)
    {
        return _cardFilters.TargetIsValid(key.Context.PlayedCard, key.Context);
            
    }
}
