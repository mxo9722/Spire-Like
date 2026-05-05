using System.Collections.Generic;
using UnityEngine;

public class ConvertStatusEffectGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    public StatusEffectInfo From { get; private set; }
    public StatusEffectInfo To { get; private set; }
    public int UpTo { get; private set; }

    public ConvertStatusEffectGA(List<CombatantView> targets, StatusEffectInfo from, StatusEffectInfo to, int upTo)
    {
        Targets = targets;
        From = from;
        To = to;
        UpTo = upTo;
    }
}
