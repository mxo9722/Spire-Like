using System.Collections.Generic;
using UnityEngine;

public class InvokeBurnGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    
    public InvokeBurnGA(List<CombatantView> targets)
    {
        Targets = targets;
    }
    
    public InvokeBurnGA(CombatantView target)
    {
        Targets = new() { target };
    }
}
