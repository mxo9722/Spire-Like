using System.Collections.Generic;
using UnityEngine;

public class IncomingAttackDamageMC : ModifierCondition
{
    [SerializeField] private bool _indirectOnly = false;

    public override bool ConditionIsMet(ModifierKey key)
    {
        if(key is AttackDamageModKey attackDamageModKey)
        {
            return attackDamageModKey.Target == attackDamageModKey.Context.GetData<List<CombatantView>>("Owner")[0];
        }

        return false;
    }

    public override void Subscribe(ConditionalModifierSystem.ModifierDelegate action, object subscriber)
    {
        if(_indirectOnly)
            ConditionalModifierSystem.Subscribe<IndirectAttackModKey>(action, subscriber, _timing);
        else
            ConditionalModifierSystem.Subscribe<AttackDamageModKey>(action, subscriber, _timing);
    }

    public override void Unsubscribe(object subscriber)
    {
        if(_indirectOnly)
            ConditionalModifierSystem.Unsubscribe<IndirectAttackModKey>(subscriber, _timing);
        else
            ConditionalModifierSystem.Unsubscribe<AttackDamageModKey>(subscriber, _timing);
    }
}
