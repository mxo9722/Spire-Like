using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffectEffect : Effect
{
    [SerializeField] private StatusEffectType _statusEffectType;
    [SerializeField] private int _stackCount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new AddStatusEffectGA(_statusEffectType, _stackCount, targets, caster);
    }
}
