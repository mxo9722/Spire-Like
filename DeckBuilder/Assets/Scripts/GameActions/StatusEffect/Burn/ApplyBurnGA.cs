using System.Collections.Generic;
using UnityEngine;

public class ApplyBurnGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    
    public ApplyBurnGA(List<CombatantView> targets)
    {
        Targets = targets;
    }
    
    public ApplyBurnGA(CombatantView target)
    {
        Targets = new() { target };
    }
}
