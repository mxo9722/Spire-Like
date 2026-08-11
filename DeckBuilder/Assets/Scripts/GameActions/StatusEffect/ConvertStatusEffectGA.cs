using System.Collections.Generic;
using UnityEngine;

public class ConvertStatusEffectGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    public StatusEffectInfo From { get; private set; }
    public StatusEffectInfo To { get; private set; }
    public EffectContext Context { get; private set; }
    public int UpTo { get; private set; }

    public ConvertStatusEffectGA(List<CombatantView> targets, StatusEffectInfo from, StatusEffectInfo to, EffectContext context, int upTo)
    {
        Targets = targets;
        From = from;
        To = to;
        Context = context;
        UpTo = upTo;
    }
}
