using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddStatusEffectGA : CombinableGameAction<AddStatusEffectGA>, IHaveCaster
{
    public StatusEffect StatusEffectType { get; private set; }
    public int StackCount { get; private set; }
    public List<CombatantView> Targets { get; private set; }
    public CombatantView Caster { get; private set; }

    public bool SkipAnimation { get; private set; } = false;

    public AddStatusEffectGA(StatusEffect statusEffectType, int stackCount, List<CombatantView> targets, CombatantView caster = null, bool skipAnimation = false)
    {
        StatusEffectType = statusEffectType;
        StackCount = stackCount;
        Targets = targets.Where(t => t.CurrentHealth > 0).ToList();
        Caster = caster;
        SkipAnimation = skipAnimation;
    }

    public void SetStackCount(int stackCount)
    {
        StackCount = stackCount;
    }

    public override bool TryCombine(GameAction other)
    {
        if(other is AddStatusEffectGA addStatusEffectGA)
        {
            if (addStatusEffectGA.StatusEffectType != StatusEffectType) return false;

            if (!addStatusEffectGA.Targets.Any(t => Targets.Contains(t)) && addStatusEffectGA.StackCount == StackCount) 
            {
                Combine(addStatusEffectGA);
                return true;
            }
            else if (addStatusEffectGA.Targets.Except(Targets).Count() == 0 && Targets.Except(addStatusEffectGA.Targets).Count() == 0)
            {
                StackCount += addStatusEffectGA.StackCount;
                return true;
            }
        }

        return false;
    }

    public override void Combine(AddStatusEffectGA other)
    {
        Targets.AddRange(other.Targets);
    }
}
