using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddStatusEffectGA : GameAction, IHaveCaster
{
    public StatusEffectInfo StatusEffectInfo { get; private set; }
    public int StackCount { get; private set; }
    public List<CombatantView> Targets { get; private set; }
    public EffectContext Context { get; private set; }
    public CombatantView Caster { get => Context.Caster; }

    public bool SkipAnimation { get; private set; } = false;

    public AddStatusEffectGA(StatusEffectInfo statusEffectInfo, int stackCount, List<CombatantView> targets, EffectContext context, bool skipAnimation = false)
    {
        StatusEffectInfo = statusEffectInfo;
        StackCount = stackCount;
        Targets = targets.Where(t => t != null && t.CurrentHealth > 0).ToList();
        Context = context;
        SkipAnimation = skipAnimation;
    }

    public void SetStackCount(int stackCount)
    {
        StackCount = stackCount;
    }
}
